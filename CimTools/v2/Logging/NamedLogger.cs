using UnityEngine;

namespace CimTools.v2.Logging
{
    /// <summary>
    /// Logs to the Unity console with your mods name.
    /// </summary>
    public class NamedLogger
    {
        private CimToolSettings _modSettings;

        public NamedLogger(CimToolSettings modSettings)
        {
            _modSettings = modSettings;
        }

        public void LogError(string message)
        {
            Debug.LogError(_modSettings.ModName + ": " + message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(_modSettings.ModName + ": " + message);
        }

        public void Log(string message)
        {
            Debug.Log(_modSettings.ModName + ": " + message);
        }
    }
}
