using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace CimTools.File
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
    [Serializable()]
    [XmlRoot(ElementName = "SavedOptions")]
    public class PersistentOptions : ExportOptionBase
    {
        /// <summary>
        /// The current version of the PersistentOptions class. This is for
        /// doing upgrades from older versions of the class if needed.
        /// </summary>
        public string OptionVersion = "0.1";
        internal string actualOptionVersion = "0.1";

        private static PersistentOptions instance = null;

        /// <summary>
        /// Gets an instance of the SavedOptions
        /// </summary>
        /// <returns>An instance of SavedOptions</returns>
        public static PersistentOptions Instance()
        {
            if (instance == null)
            {
                instance = new PersistentOptions();
            }

            return instance;
        }

        /// <summary>
        /// Change the instance used for the options.
        /// </summary>
        /// <param name="optionManager">The SavedOptions to replace the existing instance</param>
        public static void SetInstance(PersistentOptions optionManager)
        {
            if (optionManager != null)
            {
                instance = optionManager;
            }
        }
        
        /// <summary>
        /// Saves all options to the disk. Make sure you've updated the options first.
        /// </summary>
        public static OptionError Save()
        {
            OptionError error = OptionError.NoError;

            if (Settings.ModName != null)
            {
                try
                {
                    XmlSerializer xmlSerialiser = new XmlSerializer(typeof(PersistentOptions));
                    StreamWriter writer = new StreamWriter(Settings.ModName + "Options.xml");

                    xmlSerialiser.Serialize(writer, Instance());
                    writer.Close();
                }
                catch
                {
                    error = OptionError.SaveFailed;
                }
            }
            else
            {
                error = OptionError.SaveFailed;
            }

            return error;
        }

        /// <summary>
        /// Load all options from the disk.
        /// </summary>
        /// <returns>Whether loading was successful</returns>
        public static OptionError Load()
        {
            OptionError error = OptionError.NoError;

            if (System.IO.File.Exists(Settings.ModName + "Options.xml"))
            {
                XmlSerializer xmlSerialiser = new XmlSerializer(typeof(PersistentOptions));
                StreamReader reader = new StreamReader(Settings.ModName + "Options.xml");

                try
                {
                    PersistentOptions savedOptions = xmlSerialiser.Deserialize(reader) as PersistentOptions;

                    reader.Close();

                    if (savedOptions != null)
                    {
                        SetInstance(savedOptions);
                    }
                    else
                    {
                        error = OptionError.LoadFailed;
                    }
                }
                catch
                {
                    error = OptionError.LoadFailed;
                }
            }
            else
            {
                error = OptionError.FileNotFound;
            }

            return error;
        }
    }
}
