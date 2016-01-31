using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace CimTools.V1.File
{
    /// <summary>
    /// Persistent options are not saved inside the save file for a level,
    /// and persist regardless of the level that is loaded. These are good
    /// for saving options for a mod, which do not need to interact with
    /// elements within a certain level, or elements that can change between
    /// saved files.
    /// <para>
    /// All data stored in here is saved to the same folder as the game
    /// executable. This can be edited at any time by the user, and is not
    /// protected. It is stored as XML.
    /// </para>
    /// </summary>
    public class XmlFileManager
    {
        /// <summary>
        /// The current version of the PersistentOptions class. This is for
        /// doing upgrades from older versions of the class if needed.
        /// </summary>
        public string OptionVersion = "0.1";
        internal string actualOptionVersion = "0.1";

        private XmlFileOptions m_savedOptions;
        internal CimToolSettings m_modSettings;

        public XmlFileOptions Data
        {
            get { return m_savedOptions; }
        }

        public XmlFileManager(CimToolSettings modSettings)
        {
            m_savedOptions = new XmlFileOptions();
            m_modSettings = modSettings;
        }

        public ExportOptionBase.OptionError Save()
        {
            StreamWriter writer = new StreamWriter(m_modSettings.ModName + "Options.xml");

            return Save(writer);
        }
        
        /// <summary>
        /// Saves all options to the disk. Make sure you've updated the options first.
        /// </summary>
        public ExportOptionBase.OptionError Save(TextWriter writeTo)
        {
            ExportOptionBase.OptionError error = ExportOptionBase.OptionError.NoError;

            if (m_modSettings.ModName != null)
            {
                try
                {
                    XmlSerializer xmlSerialiser = new XmlSerializer(typeof(XmlFileOptions));

                    xmlSerialiser.Serialize(writeTo, m_savedOptions);
                    writeTo.Close();
                }
                catch(Exception ex)
                {
                    ex.ToString();
                    error = ExportOptionBase.OptionError.SaveFailed;
                }
            }
            else
            {
                error = ExportOptionBase.OptionError.SaveFailed;
            }

            return error;
        }

        public ExportOptionBase.OptionError Load()
        {
            ExportOptionBase.OptionError error = ExportOptionBase.OptionError.NoError;

            if (m_modSettings.ModName != null && System.IO.File.Exists(m_modSettings.ModName + "Options.xml"))
            {
                StreamReader reader = new StreamReader(m_modSettings.ModName + "Options.xml");

                error = Load(reader);
            }
            else
            {
                error = ExportOptionBase.OptionError.FileNotFound;
            }

            return error;
        }

        /// <summary>
        /// Load all options from the disk.
        /// </summary>
        /// <returns>Whether loading was successful</returns>
        public ExportOptionBase.OptionError Load(TextReader readTo)
        {
            ExportOptionBase.OptionError error = ExportOptionBase.OptionError.NoError;

            if (m_modSettings.ModName != null && System.IO.File.Exists(m_modSettings.ModName + "Options.xml"))
            {
                XmlSerializer xmlSerialiser = new XmlSerializer(typeof(XmlFileOptions));

                try
                {
                    XmlFileOptions savedOptions = xmlSerialiser.Deserialize(readTo) as XmlFileOptions;

                    readTo.Close();

                    if (savedOptions != null)
                    {
                        m_savedOptions = savedOptions;
                    }
                    else
                    {
                        error = ExportOptionBase.OptionError.LoadFailed;
                    }
                }
                catch
                {
                    error = ExportOptionBase.OptionError.LoadFailed;
                }
            }
            else
            {
                error = ExportOptionBase.OptionError.FileNotFound;
            }

            return error;
        }
    }
}
