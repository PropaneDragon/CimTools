using System;

namespace CimTools.v2.Attributes
{
    /// <summary>
    /// Specifies that the class should be saved as an XML file
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class XmlOptionsAttribute : Attribute
    {
        /// <summary>
        /// Specifies where the data is saved
        /// </summary>
        public enum OptionType
        {
            /// <summary>
            /// Saves to the xml file generated for the overall mod
            /// </summary>
            XmlFile,

            /// <summary>
            /// Saves data to the save file for each individual session
            /// </summary>
            SaveFile
        };

        /// <summary>
        /// The key to give the XML group. Specifying null will just use the class name as the key.
        /// </summary>
        public string key = null;

        /// <summary>
        /// Change where the data is saved.
        /// </summary>
        public OptionType type = OptionType.XmlFile;

        /// <summary>
        /// Specifies where the data is saved
        /// </summary>
        public XmlOptionsAttribute()
        {
        }

        /// <summary>
        /// Specifies where the data is saved
        /// </summary>
        /// <param name="key">The key to give the XML group.</param>
        public XmlOptionsAttribute(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// Specifies where the data is saved
        /// </summary>
        /// <param name="type">Change where the data is saved.</param>
        public XmlOptionsAttribute(OptionType type)
        {
            this.type = type;
        }

        /// <summary>
        /// Specifies where the data is saved
        /// </summary>
        /// <param name="key">The key to give the XML group.</param>
        /// <param name="type">Change where the data is saved.</param>
        public XmlOptionsAttribute(string key, OptionType type)
        {
            this.key = key;
            this.type = type;
        }
    }
}
