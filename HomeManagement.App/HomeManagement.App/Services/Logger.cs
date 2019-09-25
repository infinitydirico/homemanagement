using System;
using System.IO;

namespace HomeManagement.App.Services
{
    public class Logger
    {
        public static string Filename = "log.txt";

        public static string ReadLog()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return File.ReadAllText(Path.Combine(path, Filename));
        }

        public static void LogMessage(string message)
        {
            Log(message);
        }

        public static void LogException(Exception ex)
        {
            Log(ex.StackTrace);
        }

        private static void Log(string message)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var file = Path.Combine(path, Filename);
            if (!File.Exists(file))
            {
                File.Create(file);
            }

            File.AppendAllText(file, $"{DateTime.Now} : {message}{Environment.NewLine}");
        }
    }
}
