using ColossalFramework;
using ColossalFramework.Plugins;

namespace CimTools.File
{
    /// <summary>
    /// Handles system paths
    /// </summary>
    public class Path
    {
        internal static string m_savedModPath = null;

        /// <summary>
        /// Gets the full path of the mod
        /// </summary>
        /// <param name="workshopId">The workshop ID of the mod</param>
        /// <param name="modName">The name of the mod</param>
        /// <returns>The full path of the mod</returns>
        public static string GetModPath(ulong workshopId, string modName)
        {
            if (m_savedModPath == null)
            {
                PluginManager pluginManager = Singleton<PluginManager>.instance;

                foreach (PluginManager.PluginInfo pluginInfo in pluginManager.GetPluginsInfo())
                {
                    if (pluginInfo.name == modName || pluginInfo.publishedFileID.AsUInt64 == workshopId)
                    {
                        m_savedModPath = pluginInfo.modPath;
                    }
                }
            }

            return m_savedModPath;
        }
    }
}
