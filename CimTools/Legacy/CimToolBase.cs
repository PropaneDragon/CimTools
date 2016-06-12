using CimTools.Legacy.File;
using System;

namespace CimTools.Legacy
{
    /// <summary>
    /// The base for CimTools. Create a static version of this in your
    /// project and access everything through here.
    /// </summary>
    public class CimToolBase
    {
        private CimToolSettings m_modSettings = null;
        private SaveFileManager m_saveFileOptions;
        private XmlFileManager m_xmlOptions;

        /// <summary>
        /// Returns an instance of CimToolSettings
        /// </summary>
        [Obsolete("CimToolSettings in v1 has been replaced with CimToolSettings using XmlOptionsAttribute in v2", false)]
        public CimToolSettings ModSettings
        {
            get { return m_modSettings; }
        }

        /// <summary>
        /// Returns an instance of SaveFileManager
        /// </summary>
        [Obsolete("SaveFileManager in v1 has been replaced with SaveFileManager using XmlOptionsAttribute in v2", false)]
        public SaveFileManager SaveFileOptions
        {
            get { return m_saveFileOptions; }
        }

        /// <summary>
        /// Returns an instance of XmlFileManager
        /// </summary>
        [Obsolete("XmlFileManager in v1 has been replaced with XmlFileManager using XmlOptionsAttribute in v2", false)]
        public XmlFileManager XMLFileOptions
        {
            get { return m_xmlOptions; }
        }

        /// <summary>
        /// Create a new CimToolBase with your mod settings. This will create
        /// instances of useful tools and you can access them from this class.
        /// </summary>
        /// <param name="modSettings">Your mod settings</param>
        [Obsolete("CimToolBase in v1 has been replaced with CimToolBase in v2", false)]
        public CimToolBase(CimToolSettings modSettings)
        {
            m_modSettings = modSettings;
            m_saveFileOptions = new SaveFileManager(modSettings);
            m_xmlOptions = new XmlFileManager(modSettings);

            m_xmlOptions.Load();
        }
    }
}
