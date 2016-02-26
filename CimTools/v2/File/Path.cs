using ColossalFramework;
using ColossalFramework.Plugins;
using System.Collections.Generic;

namespace CimTools.v2.File
{
    /// <summary>
    /// Handles system paths
    /// </summary>
    public class Path
    {
        internal string m_savedModPath = null;
        internal CimToolSettings m_settings = null;

        public Path(CimToolSettings settings)
        {
            m_settings = settings;
        }

        /// <summary>
        /// Gets the full path of the mod
        /// </summary>
        /// <param name="workshopId">The workshop ID of the mod</param>
        /// <param name="modName">The name of the mod</param>
        /// <returns>The full path of the mod</returns>
        public string GetModPath(ulong workshopId, string modName)
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

        public string GetModPath()
        {
            return GetModPath(m_settings.WorkshopID ?? 0, m_settings.ModName);
        }
    }
}
