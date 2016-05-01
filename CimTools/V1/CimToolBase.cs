using CimTools.V1.File;
using CimTools.V1.Utilities;
using CimTools.V1.Workshop;

namespace CimTools.V1
{
    /// <summary>
    /// The base for CimTools. Create a static version of this in your
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
        private ModOptionUtilities m_modOptions;
        private DetailedLogger m_detailedLogger;
        private NamedLogger m_namedLogger;
        private Translation m_translation;
        private UIUtilities m_uiUtilities;

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

        public ModOptionUtilities ModOptions
        {
            get { return m_modOptions; }
        }

        public DetailedLogger DetailedLogger
        {
            get { return m_detailedLogger; }
        }

        public NamedLogger NamedLogger
        {
            get { return m_namedLogger; }
        }

        public Translation Translation
        {
            get { return m_translation; }
        }

        public UIUtilities UIUtilities
        {
            get { return m_uiUtilities; }
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
            m_path = new Path(modSettings);
            m_version = new Version(modSettings.ModAssembly);
            m_saveFileOptions = new SaveFileManager(modSettings);
            m_xmlOptions = new XmlFileManager(modSettings);
            m_changelog = new Changelog(modSettings);
            m_modOptions = new ModOptionUtilities(this);
            m_detailedLogger = new DetailedLogger(modSettings);
            m_namedLogger = new NamedLogger(modSettings);
            m_translation = new Translation(this);
            m_uiUtilities = new UIUtilities();

            m_xmlOptions.Load();
        }
    }
}
