using CimTools.v2.File;
using CimTools.v2.Logging;
using CimTools.v2.Utilities;
using CimTools.v2.Workshop;

namespace CimTools.v2
{
    /// <summary>
    /// The base for CimTools. Create a static/instanced version of this in your
    /// project and access everything through here.
    /// </summary>
    public class CimToolBase
    {
        private CimToolSettings m_modSettings = null;
        private SpriteUtilities m_spriteUtilities;
        private Changelog m_changelog;
        private SaveFileManager m_saveFileOptions;
        private XmlFileManager m_xmlOptions;
        private Path m_path;
        private Version m_version;
        private ModOptionPanelUtilities m_modOptions;
        private DetailedLogger m_detailedLogger;
        private NamedLogger m_namedLogger;
        private Translation m_translation;

        /// <summary>
        /// Returns an instance of CimToolSettings
        /// </summary>
        public CimToolSettings ModSettings
        {
            get { return m_modSettings; }
        }

        /// <summary>
        /// Returns an instance of SpriteUtilities
        /// </summary>
        public SpriteUtilities SpriteUtilities
        {
            get { return m_spriteUtilities; }
        }

        /// <summary>
        /// Returns an instance of Changelog
        /// </summary>
        public Changelog Changelog
        {
            get { return m_changelog; }
        }

        /// <summary>
        /// Returns an instance of SaveFileManager
        /// </summary>
        public SaveFileManager SaveFileOptions
        {
            get { return m_saveFileOptions; }
        }

        /// <summary>
        /// Returns an instance of XmlFileManager
        /// </summary>
        public XmlFileManager XMLFileOptions
        {
            get { return m_xmlOptions; }
        }

        /// <summary>
        /// Returns an instance of Path
        /// </summary>
        public Path Path
        {
            get { return m_path; }
        }

        /// <summary>
        /// Returns an instance of Version
        /// </summary>
        public Version Version
        {
            get { return m_version; }
        }

        public ModOptionPanelUtilities ModPanelOptions
        {
            get { return m_modOptions; }
        }

        /// <summary>
        /// Returns an instance of the DetailedLogger
        /// </summary>
        public DetailedLogger DetailedLogger
        {
            get { return m_detailedLogger; }
        }

        /// <summary>
        /// Returns an instance of the NamedLogger
        /// </summary>
        public NamedLogger NamedLogger
        {
            get { return m_namedLogger; }
        }

        /// <summary>
        /// Returns an instance of Translation
        /// </summary>
        public Translation Translation
        {
            get { return m_translation; }
        }

        /// <summary>
        /// Create a new CimToolBase with your mod settings. This will create
        /// instances of useful tools and you can access them from this class.
        /// </summary>
        /// <param name="modSettings">Your mod settings</param>
        public CimToolBase(CimToolSettings modSettings)
        {
            m_modSettings = modSettings;

            m_spriteUtilities = new SpriteUtilities();
            m_path = new Path(this);
            m_version = new Version(this);
            m_detailedLogger = new DetailedLogger(this);
            m_namedLogger = new NamedLogger(this);
            m_translation = new Translation(this);
            m_xmlOptions = new XmlFileManager(this);
            m_saveFileOptions = new SaveFileManager(this);
            m_changelog = new Changelog(this);
            m_modOptions = new ModOptionPanelUtilities(this);
        }
    }
}
