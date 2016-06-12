using UnityEngine;

namespace CimTools.v2.Logging
{
    /// <summary>
    /// Logs to the Unity console with your mods name.
    /// </summary>
    public class NamedLogger
    {
        private CimToolBase _toolBase;

        public NamedLogger(CimToolBase toolBase)
        {
            _toolBase = toolBase;
        }

        public void LogError(string message)
        {
            Debug.LogError(_toolBase.ModSettings.ReadableName + ": " + message);
            _toolBase.DetailedLogger.LogError(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(_toolBase.ModSettings.ReadableName + ": " + message);
            _toolBase.DetailedLogger.LogWarning(message);
        }

        public void Log(string message)
        {
            Debug.Log(_toolBase.ModSettings.ReadableName + ": " + message);
            _toolBase.DetailedLogger.Log(message);
        }
    }
}
