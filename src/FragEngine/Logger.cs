using System;
using System.IO;
using System.Text;

namespace FragEngine
{
    public class Logger
    {
        public static readonly Logger Instance = new Logger();

        private readonly string _logFileName = $"fragengine_${DateTime.UtcNow.Ticks}.log";

        private Logger()
        {
            
        }

        public void LogError(Exception e, string subsystem = "")
        {
            Write("ERROR", e.Message, subsystem, e);
        }

        public void LogError(string message, string subsystem = "", params object[] messageArgs)
        {
            Write("ERROR", message, subsystem, messageArgs);
        }

        public void LogMessage(string message, string subsystem = "", params object[] messageArgs)
        {
            Write("INFO", message, subsystem, messageArgs);
        }

        public void LogWarning(string message, string subsystem = "", params object[] messageArgs)
        {
            Write("WARN", message, subsystem, messageArgs);
        }

        private void Write(string level, string message, string subsystem = "", params object[] messageArgs)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(level);

            if (String.IsNullOrWhiteSpace(subsystem))
                strBuilder.Append($" [{subsystem}]");

            strBuilder.Append($" {message}");

            if (messageArgs != null)
            {
                foreach (var obj in messageArgs)
                {
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        strBuilder.Append($" {prop.Name} = {prop.GetValue(obj)}");
                    }
                }
            }
            Write(strBuilder.ToString());
        }

        private void Write(string data)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _logFileName);
            using (var file = new StreamWriter(filePath, true))
            {
                file.WriteLine(data);
                file.Close();
            }
        }
    }
}
