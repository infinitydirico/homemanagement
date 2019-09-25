using System;
using System.IO;

namespace HomeManagement.App.Services
{
    public class Logger
    {
        public static string Filename = "log.txt";

        public static string ReadLog() => File.ReadAllText(GetFullPath());

        public static void LogMessage(string message) => Log(message);

        public static void LogException(Exception ex) => Log(ex.StackTrace);

        public static void Clear()
        {
            var file = GetFullPath();
            if (File.Exists(file))
            {
                File.WriteAllText(file, string.Empty);
            }
        }

        private static void Log(string message)
        {
            var file = GetFullPath();
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }

            File.AppendAllText(file, $"{DateTime.Now} : {message}{Environment.NewLine}");
        }

        private static string GetFullPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Filename);
    }
}
