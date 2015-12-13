using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CimTools.File
{
    public partial class ExportOptionBase
    {
        /// <summary>
        /// Any errors that could occur in the options.
        /// </summary>
        public enum OptionError
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

        /// <summary>
        /// Contains all saveable data. You shouldn't have to modify this
        /// manually, and instead should use the SetValue and GetValue methods.
        /// </summary>
        public List<SavedGroup> SavedData = new List<SavedGroup>();

        private static readonly string emptyGroupName = "";

        internal SavedGroup GetGroup(string name, bool createIfNotExists = false)
        {
            SavedGroup returnGroup = null;

            if (name == null)
            {
                name = emptyGroupName;
            }

            foreach (SavedGroup group in SavedData)
            {
                if (group.name == name)
                {
                    returnGroup = group;
                }
            }

            if (returnGroup == null && createIfNotExists)
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
                    if (GetValue<T>(element.name, out valueOut, groupName) == OptionError.NoError)
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
        public OptionError GetValue<T>(string name, out T value, string groupName = null)
        {
            SavedElement foundElement = null;
            SavedGroup foundGroup = GetGroup(groupName);
            OptionError error = OptionError.NoError;

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
                            error = OptionError.CastFailed;
                        }
                    }
                    catch
                    {
                        error = OptionError.CastFailed;
                    }
                }
                else
                {
                    error = OptionError.OptionNotFound;
                }
            }
            else
            {
                error = OptionError.GroupNotFound;
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

                foreach (SavedElement element in foundGroup.elements)
                {
                    if (element.name == name)
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
