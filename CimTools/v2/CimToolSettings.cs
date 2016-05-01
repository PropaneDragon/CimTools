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
        private string m_readableName = null;
        private string m_internalName = null;
        private ulong? m_workshopID = null;
        private List<Assembly> m_assembly = null;

        /// <summary>
        /// The folder name of the mod.
        /// </summary>
        public string ModName
        {
            get { return m_internalName; }
        }

        /// <summary>
        /// The name of the mod. This should be the same as the workshop
        /// name.
        /// </summary>
        public string ReadableName
        {
            get { return m_readableName; }
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
        public Assembly MainAssembly
        {
            get { return m_assembly[0]; }
        }

        /// <summary>
        /// All assemblies in the mod
        /// </summary>
        public List<Assembly> Assemblies
        {
            get { return m_assembly; }
        }

        /// <summary>
        /// Initialises the settings for the rest of the mod
        /// </summary>
        /// <param name="readableName">How you want the mods name to appear in game</param>
        /// <param name="modName">The actual file name of the mod.</param>
        /// <param name="modAssembly">The assembly of the mod. If null it will just use GetCallingAssembly.</param>
        /// <param name="workshopId">The workshop ID of the mod (if you have one).</param>
        public CimToolSettings(string readableName, string modName = null, Assembly modAssembly = null, ulong? workshopId = null)
        {
            m_readableName = readableName;
            m_internalName = modName;
            m_workshopID = workshopId;

            if(m_assembly == null)
            {
                m_assembly = new List<Assembly>();
            }

            if(modAssembly == null)
            {
                m_assembly.Add(Assembly.GetCallingAssembly());
            }
            else
            {
                m_assembly.Add(modAssembly);
            }
        }

        public void AddAssembly(Assembly assemblyToAdd)
        {
            if(!m_assembly.Contains(assemblyToAdd))
            {
                m_assembly.Add(assemblyToAdd);
            }
        }
    }
}
