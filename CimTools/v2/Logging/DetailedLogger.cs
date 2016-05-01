using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace CimTools.v2.Logging
{
    /// <summary>
    /// Allows you to log detailed messages about your mod in its own file.
    /// </summary>
    public class DetailedLogger
    {
        /// <summary>
        /// The type of log to create.
        /// </summary>
        public enum LogType { Message, Warning, Error };

        private string _fileName;
        private Timer _saveTimer = new Timer(10000);
        private List<KeyValuePair<LogType, string>> _queuedMessages = new List<KeyValuePair<LogType, string>>();

        public DetailedLogger(CimToolBase toolBase)
        {
            _saveTimer.Elapsed += SaveTimer_Elapsed;
            _saveTimer.Enabled = true;
            _saveTimer.AutoReset = true;

            _fileName = toolBase.ModSettings.ModName + "-detailed.log";
            
            System.IO.File.Delete(_fileName);
            System.IO.File.CreateText(_fileName);
        }

        private void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(_queuedMessages.Count > 0)
            {
                StreamWriter logWriter = System.IO.File.AppendText(_fileName);

                foreach(KeyValuePair<LogType, string> messagePair in _queuedMessages)
                {
                    LogType logType = messagePair.Key;
                    string message = messagePair.Value;
                    string outputMessage = "";

                    if (logType == LogType.Message) outputMessage += "[i] ";
                    if (logType == LogType.Warning) outputMessage += "[!] ";
                    if (logType == LogType.Error) outputMessage += "[x] ";

                    outputMessage += message;

                    logWriter.WriteLine(outputMessage);
                }

                logWriter.Close();
                _queuedMessages.Clear();
            }
        }

        public void LogError(string message)
        {
            _queuedMessages.Add(new KeyValuePair<LogType, string>(LogType.Error, message));
        }

        public void LogWarning(string message)
        {
            _queuedMessages.Add(new KeyValuePair<LogType, string>(LogType.Warning, message));
        }

        public void Log(string message)
        {
            _queuedMessages.Add(new KeyValuePair<LogType, string>(LogType.Message, message));
        }
    }
}
