using NLog;
using NLog.Config;
using NLog.Targets;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V132.Performance;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V132.DevToolsSessionDomains;
using Performance = OpenQA.Selenium.DevTools.V132.Performance;

namespace Performancer
{
    public class CoreManager
    {
        public static IWebDriver webDriver;
        public static IDevToolsSession session;
        public static DevToolsSessionDomains devToolsSession;
        protected static readonly Logger Logger;

        static CoreManager()
        {
            Logger = LogManager.GetCurrentClassLogger();
            ConfigureLogger();
        }

        public CoreManager()
        {
            webDriver = new ChromeDriver();
            webDriver.Manage().Window.Maximize();
            IDevTools devTools = webDriver as IDevTools;
            session = devTools.GetDevToolsSession();
            devToolsSession = session.GetVersionSpecificDomains<DevToolsSessionDomains>();
            devToolsSession.Performance.Enable(new Performance.EnableCommandSettings());
            ConfigureLogger();
        }

        private static void ConfigureLogger()
        {
            string logFileName = $"{DateTime.Now:ddMMyyyy_HHmmss}.log";
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", logFileName);

            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget("logfile")
            {
                FileName = logFilePath,
                Layout = "${longdate} | ${level} | ${message} | ${exception}"
            };

            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;
        }

        public static PerformanceMetrics Metrics
        {
            get
            {
                var metricsArray = devToolsSession.Performance.GetMetrics().Result.Metrics;
                var perfMetrics = new PerformanceMetrics();

                foreach (var metric in metricsArray)
                {
                    switch (metric.Name)
                    {
                        case "Documents": perfMetrics.Documents = metric.Value; break;
                        case "Frames": perfMetrics.Frames = metric.Value; break;
                        case "JSEventListeners": perfMetrics.JSEventListeners = metric.Value; break;
                        case "LayoutObjects": perfMetrics.LayoutObjects = metric.Value; break;
                        case "Nodes": perfMetrics.Nodes = metric.Value; break;
                        case "ScriptDuration": perfMetrics.ScriptDuration = metric.Value; break;
                        case "TaskDuration": perfMetrics.TaskDuration = metric.Value; break;
                        case "JSHeapUsedSize": perfMetrics.JSHeapUsedSize = metric.Value; break;
                        case "JSHeapTotalSize": perfMetrics.JSHeapTotalSize = metric.Value; break;
                    }
                }
                return perfMetrics;
            }
        }
    }

    public class PerformanceMetrics
    {
        public double Documents { get; set; }
        public double Frames { get; set; }
        public double JSEventListeners { get; set; }
        public double LayoutObjects { get; set; }
        public double Nodes { get; set; }
        public double ScriptDuration { get; set; }
        public double TaskDuration { get; set; }
        public double JSHeapUsedSize { get; set; }
        public double JSHeapTotalSize { get; set; }
    }
}
