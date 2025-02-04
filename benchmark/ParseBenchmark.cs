using BenchmarkDotNet.Attributes;
using GrokNet;

namespace Benchmark
{
    [MemoryDiagnoser(false)]
    public class ParseBenchmark
    {
        private static readonly Grok _grokEmpty = new("");
        private static readonly Grok _grokLog = new("%{MONTHDAY:month}-%{MONTHDAY:day}-%{MONTHDAY:year} %{TIME:timestamp};%{WORD:id};%{LOGLEVEL:loglevel};%{WORD:func};%{GREEDYDATA:msg}");
        private static readonly Grok _grokCustom = new("%{ZIPCODE:zipcode}:%{EMAILADDRESS:email}");

        [Benchmark]
        public void Empty()
        {
            _ = _grokEmpty.Parse("");
        }

        [Benchmark]
        public void Custom()
        {
            _ = _grokCustom.Parse("06590:halil.i.kocaoz@gmail.com");
        }

        [Benchmark]
        public void Log()
        {
            _ = _grokLog.Parse(@"06-21-19 21:00:13:589241;15;INFO;main;DECODED: 775233900043 DECODED BY: 18500738 DISTANCE: 1.5165
                06-21-19 21:00:13:589265;156;WARN;main;DECODED: 775233900043 EMPTY DISTANCE: --------");
        }

        [Params("DBG", "INF", "WARN", "ERR")]
        public string LogLevel { get; set; }

        [Benchmark]
        public void LogWithParam()
        {
            GrokResult grokResult = _grokLog.Parse($@"06-21-19 21:00:13:589241;15;INFO;main;DECODED: 775233900043 DECODED BY: 18500738 DISTANCE: 1.5165
                06-21-19 21:00:13:589265;156;{LogLevel};main;DECODED: 775233900043 EMPTY DISTANCE: --------");

            bool resut = (string)grokResult[0].Value == LogLevel;
        }

        [Benchmark]
        public void EmptyLocal()
        {
            Grok grokEmptyLocal = new Grok("");
            _ = grokEmptyLocal.Parse("");
        }

        [Benchmark]
        public void CustomLocal()
        {
            Grok grokCustomLocal = new Grok("%{ZIPCODE:zipcode}:%{EMAILADDRESS:email}");
            _ = grokCustomLocal.Parse("06590:halil.i.kocaoz@gmail.com");
        }

        [Benchmark]
        public void LogLocal()
        {
            Grok grokLogLocal = new Grok("%{MONTHDAY:month}-%{MONTHDAY:day}-%{MONTHDAY:year} %{TIME:timestamp};%{WORD:id};%{LOGLEVEL:loglevel};%{WORD:func};%{GREEDYDATA:msg}");
            _ = grokLogLocal.Parse(@"06-21-19 21:00:13:589241;15;INFO;main;DECODED: 775233900043 DECODED BY: 18500738 DISTANCE: 1.5165
                06-21-19 21:00:13:589265;156;WARN;main;DECODED: 775233900043 EMPTY DISTANCE: --------");
        }
    }
}
