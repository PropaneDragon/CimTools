using System.Collections.Generic;
using System.Reflection;

namespace CimTools.v2
{
    /// <summary>
    /// Global settings for CimTools.v2. Set these up before doing anything
    /// with the tools, or elements may not work properly.
    /// </summary>
    public class CimToolSettings
    {
        internal string m_modName = null;
        internal string m_modFolderName = null;
        internal ulong? m_workshopID = null;
        internal Assembly m_assembly = null;

        /// <summary>
        /// The folder name of the mod.
        /// </summary>
        public string ModFolderName
        {
            get { return m_modFolderName; }
        }

        /// <summary>
        /// The name of the mod. This should be the same as the workshop
        /// name.
        /// </summary>
        public string ModName
        {
            get { return m_modName; }
        }

        /// <summary>
        /// Workshop ID of the mod.
        /// </summary>
        public ulong? WorkshopID
        {
            get { return m_workshopID; }
        }

        /// <summary>
        /// Assembly of the mod.
        /// </summary>
        public Assembly ModAssembly
        {
            get { return m_assembly; }
        }

        /// <summary>
        /// Initialises the settings for the rest of the mod
        /// </summary>
        /// <param name="readableName">How you want the mods name to appear in game</param>
        /// <param name="modFolderName">The name of the folder this mod belongs to (in the Mods folder, not on the workshop).</param>
        /// <param name="modAssembly">The assembly of the mod. If null it will just use GetCallingAssembly.</param>
        /// <param name="workshopId">The workshop ID of the mod (if you have one).</param>
        public CimToolSettings(string readableName, string modFolderName = null, Assembly modAssembly = null, ulong? workshopId = null)
        {
            m_modName = readableName;
            m_modFolderName = modFolderName;
            m_workshopID = workshopId;

            if(modAssembly == null)
            {
                m_assembly = Assembly.GetCallingAssembly();
            }
            else
            {
                m_assembly = modAssembly;
            }
        }
    }
}
