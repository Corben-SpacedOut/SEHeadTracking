using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadTrackingPlugin
{
    public static class Log
    {
        public const string NAME = "HeadTrackingPlugin.log";

        // Logs go to %APPDATA%/SpaceEngineers
        private static Lazy<string> _LogFile = new Lazy<string>(() =>
        {
            var appdata = Environment.GetEnvironmentVariable("appdata");
            var file = appdata + $"/SpaceEngineers/{NAME}";
            System.IO.File.WriteAllText(file, $"Logging started.\n");
            return file;
        });

        private static string LogFile { get { return _LogFile.Value; } }

        public static void Info(string msg)
        {
            System.IO.File.AppendAllText(LogFile, DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + msg + "\n");
        }

        public static void Error(string msg)
        {
            Info("ERROR: " + msg);
        }
    }
}
