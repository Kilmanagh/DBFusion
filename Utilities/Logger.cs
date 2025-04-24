namespace DBFusion.Utilities
{


    using System;
    using System.IO;

    public static class Logger
    {
        private static readonly string LogFilePath = "logs/log.txt"; // Path for log file

        static Logger()
        {
            // Ensure the logs directory exists
            var logDirectory = Path.GetDirectoryName(LogFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        // Log informational messages
        public static void LogInfo(string message)
        {
            Log("INFO", message);
        }

        // Log warning messages
        public static void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        // Log error messages, with optional exception details
        public static void LogError(string message, Exception? ex = null)
        {
            var errorMessage = ex == null
                ? message
                : $"{message}\nException: {ex.Message}\nStackTrace: {ex.StackTrace}";
            Log("ERROR", errorMessage);
        }

        // Core logging method
        private static void Log(string level, string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";

            // Write log to the log file
            if (!string.IsNullOrEmpty(LogFilePath))
            {
                File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
            }

            // Optionally write log to the console for development visibility
            Console.WriteLine(logMessage);
        }
    }
}

