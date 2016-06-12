using CimTools.v2.Data;
using CimTools.v2.Utilities;
using CimTools.v2.Logging;
using CimTools.v2.Workshop;
using CimTools.v2.File;

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
        private UIUtilities m_uiUtilities;
        private Changelog m_changelog;
        private SaveFileManager m_saveFileOptions;
        private XmlFileManager m_xmlOptions;
        private Path m_path;
        private Version m_version;
        private ModOptionPanelUtilities m_modOptions;
        private DetailedLogger m_detailedLogger;
        private NamedLogger m_namedLogger;
        private Translation m_translation;
        private Strings m_strings;

        /// <summary>
        /// Settings CimTools uses
        /// </summary>
        public CimToolSettings ModSettings
        {
            get { return m_modSettings; }
        }

        /// <summary>
        /// Utilities for retrieving and manipulating sprites and atlases
        /// </summary>
        public SpriteUtilities SpriteUtilities
        {
            get { return m_spriteUtilities; }
        }

        /// <summary>
        /// Utilities for dealing with the UI
        /// </summary>
        public UIUtilities UIUtilities
        {
            get { return m_uiUtilities; }
        }

        /// <summary>
        /// Handles parsing and displaying the changelog of your mod automatically
        /// </summary>
        public Changelog Changelog
        {
            get { return m_changelog; }
        }

        /// <summary>
        /// Handles saving and loading to and from the game save file
        /// </summary>
        public SaveFileManager SaveFileOptions
        {
            get { return m_saveFileOptions; }
        }

        /// <summary>
        /// Handles saving and loading to and from XML
        /// </summary>
        public XmlFileManager XMLFileOptions
        {
            get { return m_xmlOptions; }
        }

        /// <summary>
        /// Handles system paths for the mod
        /// </summary>
        public Path Path
        {
            get { return m_path; }
        }

        /// <summary>
        /// Handles version numbers and strings for the mod
        /// </summary>
        public Version Version
        {
            get { return m_version; }
        }

        /// <summary>
        /// Handles items on the settings panel in Cities
        /// </summary>
        public ModOptionPanelUtilities ModPanelOptions
        {
            get { return m_modOptions; }
        }

        /// <summary>
        /// Handles logging to a separate text file with the mod name for detailed debugging
        /// </summary>
        public DetailedLogger DetailedLogger
        {
            get { return m_detailedLogger; }
        }

        /// <summary>
        /// Handles logging to Unity with the mod name
        /// </summary>
        public NamedLogger NamedLogger
        {
            get { return m_namedLogger; }
        }

        /// <summary>
        /// Handles translation and localisation of strings
        /// </summary>
        public Translation Translation
        {
            get { return m_translation; }
        }

        /// <summary>
        /// Contains all constant strings the tool uses
        /// </summary>
        public Strings Strings
        {
            get { return m_strings; }
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
            m_uiUtilities = new UIUtilities();
            m_strings = new Strings(this);
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
