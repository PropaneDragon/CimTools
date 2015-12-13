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
    public class PersistentOptions
    {
        /// <summary>
        /// Data that gets saved to disk. You shouldn't have to modify this
        /// manually, and instead should use the SetValue and GetValue methods.
        /// </summary>
        public List<SavedGroup> SavedData = new List<SavedGroup>();

        /// <summary>
        /// The current version of the PersistentOptions class. This is for
        /// doing upgrades from older versions of the class if needed.
        /// </summary>
        public string OptionVersion = "0.1";
        internal string actualOptionVersion = "0.1";

        /// <summary>
        /// Any errors that could occur in the options.
        /// </summary>
        public enum PersistentOptionError
        {
            /// <summary>
            /// No error has occurred.
            /// </summary>
            NoError,

            /// <summary>
            /// Could not save the data.
            /// </summary>
            SaveFailed,

            /// <summary>
            /// Could not load the data.
            /// </summary>
            LoadFailed,

            /// <summary>
            /// The saved data file could not be found.
            /// </summary>
            FileNotFound,

            /// <summary>
            /// The group could not be retrieved.
            /// </summary>
            GroupNotFound,

            /// <summary>
            /// The option could not be retrieved.
            /// </summary>
            OptionNotFound,

            /// <summary>
            /// Could not cast the option to the specified type.
            /// </summary>
            CastFailed
        }

        private static PersistentOptions instance = null;
        private static readonly string emptyGroupName = "";

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
        public static PersistentOptionError Save()
        {
            PersistentOptionError error = PersistentOptionError.NoError;

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
                    error = PersistentOptionError.SaveFailed;
                }
            }
            else
            {
                error = PersistentOptionError.SaveFailed;
            }

            return error;
        }

        /// <summary>
        /// Load all options from the disk.
        /// </summary>
        /// <returns>Whether loading was successful</returns>
        public static PersistentOptionError Load()
        {
            PersistentOptionError error = PersistentOptionError.NoError;

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
                        error = PersistentOptionError.LoadFailed;
                    }
                }
                catch
                {
                    error = PersistentOptionError.LoadFailed;
                }
            }
            else
            {
                error = PersistentOptionError.FileNotFound;
            }

            return error;
        }

        internal SavedGroup GetGroup(string name, bool createIfNotExists = false)
        {
            SavedGroup returnGroup = null;

            if (name == null)
            {
                name = emptyGroupName;
            }

            foreach(SavedGroup group in SavedData)
            {
                if(group.name == name)
                {
                    returnGroup = group;
                }
            }

            if(returnGroup == null && createIfNotExists)
            {
                returnGroup = new SavedGroup() { name = name };
                SavedData.Add(returnGroup);
            }

            return returnGroup;
        }

        /// <summary>
        /// Gets all values of type T under the specified group name.
        /// </summary>
        /// <typeparam name="T">The type of the items to return.</typeparam>
        /// <param name="groupName">The group to search under.</param>
        /// <returns></returns>
        public Dictionary<string, T> GetValues<T>(string groupName)
        {
            Dictionary<string, T> returnValues = new Dictionary<string, T>();
            SavedGroup foundGroup = GetGroup(groupName);

            if (foundGroup != null)
            {
                foreach (SavedElement element in foundGroup.elements)
                {
                    T valueOut;
                    if(GetValue<T>(element.name, out valueOut, groupName) == PersistentOptionError.NoError)
                    {
                        returnValues.Add(element.name, valueOut);
                    }
                }
            }

            return returnValues;
        }

        /// <summary>
        /// Gets a stored value of type T.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="name">The unique name of the data.</param>
        /// <param name="value">The output value of the data.</param>
        /// <param name="groupName">The name of the group to load the data from.</param>
        /// <returns>Whether the data could be retrieved or not.</returns>
        public PersistentOptionError GetValue<T>(string name, out T value, string groupName = null)
        {
            SavedElement foundElement = null;
            SavedGroup foundGroup = GetGroup(groupName);
            PersistentOptionError error = PersistentOptionError.NoError;

            value = default(T);

            if (foundGroup != null)
            {
                foreach (SavedElement element in foundGroup.elements)
                {
                    if (element.name == name)
                    {
                        foundElement = element;
                        break;
                    }
                }

                if (foundElement != null)
                {
                    try
                    {
                        if (foundElement.value.GetType() == typeof(T))
                        {
                            value = (T)Convert.ChangeType(foundElement.value, typeof(T));
                        }
                        else
                        {
                            error = PersistentOptionError.CastFailed;
                        }
                    }
                    catch
                    {
                        error = PersistentOptionError.CastFailed;
                    }
                }
                else
                {
                    error = PersistentOptionError.OptionNotFound;
                }
            }
            else
            {
                error = PersistentOptionError.GroupNotFound;
            }

            return error;
        }

        /// <summary>
        /// Sets a value to be saved to disk automatically.
        /// </summary>
        /// <typeparam name="T">The type of the item to set.</typeparam>
        /// <param name="name">The unique name of the data.</param>
        /// <param name="value">The input value of the data.</param>
        /// <param name="groupName">The name of the group to save this data in.</param>
        public void SetValue<T>(string name, T value, string groupName = null)
        {
            SavedGroup foundGroup = GetGroup(groupName, true);

            if (foundGroup != null)
            {
                bool updatedElement = false;

                foreach(SavedElement element in foundGroup.elements)
                {
                    if(element.name == name)
                    {
                        element.value = value;
                        updatedElement = true;
                    }
                }

                if (!updatedElement)
                {
                    foundGroup.elements.Add(new SavedElement() { name = name, value = value });
                }
            }
        }
    }

    /// <summary>
    /// Container for saving data to disk.
    /// </summary>
    [Serializable()]
    public class SavedElement
    {
        /// <summary>
        /// Unique item name
        /// </summary>
        [XmlElement("Name", IsNullable = false)]
        public string name = "";

        /// <summary>
        /// Value of the item
        /// </summary>
        [XmlElement("Value", IsNullable = false)]
        public object value = default(object);
    }

    /// <summary>
    /// Container for saving groups of data to disk.
    /// </summary>
    [Serializable()]
    public class SavedGroup
    {
        /// <summary>
        /// Unique group name
        /// </summary>
        [XmlElement("Name", IsNullable = false)]
        public string name = "";

        /// <summary>
        /// SavedElements in the group
        /// </summary>
        [XmlElement("Element", IsNullable = false)]
        public List<SavedElement> elements = new List<SavedElement>();
    }
}
