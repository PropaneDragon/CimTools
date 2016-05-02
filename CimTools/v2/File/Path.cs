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
        internal CimToolBase m_toolBase = null;

        public Path(CimToolBase toolBase)
        {
            m_toolBase = toolBase;
        }

        /// <summary>
        /// Gets the full path of a particular mod
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

        /// <summary>
        /// Gets the full path of the mod
        /// </summary>
        /// <returns>The full path of the mod</returns>
        public string GetModPath()
        {
            return GetModPath(m_toolBase.ModSettings.WorkshopID ?? 0, m_toolBase.ModSettings.ModName);
        }
    }
}
