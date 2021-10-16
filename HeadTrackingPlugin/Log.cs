using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.FileSystem;

namespace HeadTrackingPlugin
{
    public static class Log
    {
        private const string FileName = "HeadTrackingPlugin.log";
        private static string FilePath => Path.Combine(MyFileSystem.UserDataPath, FileName);

        // Logs go to %APPDATA%/SpaceEngineers
        private static Lazy<string> _LogFile = new Lazy<string>(() =>
        {
            System.IO.File.WriteAllText(FilePath, $"Logging started.\n");
            return FilePath;
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
