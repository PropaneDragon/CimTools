using CimTools.v2.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace CimTools.v2.Utilities
{
    public delegate void LanguageChanged(string languageIdentifier);

    /// <summary>
    /// Handles localisation for a mod.
    /// </summary>
    public class Translation
    {
        public event LanguageChanged OnLanguageChanged;

        protected CimToolBase _toolBase = null;
        protected List<Language> _languages = new List<Language>();

        public Translation(CimToolBase toolBase)
        {
            _toolBase = toolBase;
        }

        /// <summary>
        /// Generates a language template for the mod
        /// </summary>
        public void GenerateLanguageTemplate()
        {
            List<FieldInfo> translatableFields = GetTranslatableFields(_toolBase.ModSettings.ModAssembly);
            Language exportableLanguage = new Language() { _readableName = "Exported Language", _uniqueName = "export" };

            foreach (FieldInfo translatableField in translatableFields)
            {
                object[] foundAttributes = translatableField.GetCustomAttributes(typeof(TranslatableAttribute), false);

                if (foundAttributes.Length > 0)
                {
                    TranslatableAttribute translatableInfo = foundAttributes[0] as TranslatableAttribute;
                    exportableLanguage._conversionDictionary.Add(translatableInfo.identifier, "");
                }
            }

            XmlSerializer xmlSerialiser = new XmlSerializer(typeof(Language));
            StreamWriter writer = new StreamWriter("exportedLanguage.xml");

            xmlSerialiser.Serialize(writer, exportableLanguage);
            writer.Close();
        }

        /// <summary>
        /// Gets all fields that can be translated from the specified assembly.
        /// </summary>
        /// <param name="containingAssembly">The assembly to get the translatable fields from</param>
        /// <returns>A list of fields which can be translated</returns>
        internal List<FieldInfo> GetTranslatableFields(Assembly containingAssembly)
        {
            List<FieldInfo> translatableFields = new List<FieldInfo>();

            foreach (Type individualType in containingAssembly.GetTypes())
            {
                FieldInfo[] fields = individualType.GetFields(BindingFlags.Public | BindingFlags.Static);

                foreach (FieldInfo field in fields)
                {
                    object[] foundAttributes = field.GetCustomAttributes(typeof(TranslatableAttribute), false);

                    if (foundAttributes.Length > 0)
                    {
                        translatableFields.Add(field);
                    }
                }
            }

            return translatableFields;
        }

        /// <summary>
        /// Loads all languages from the Locale folder
        /// </summary>
        public void LoadLanguages()
        {
            string basePath = _toolBase.Path.GetModPath();

            if (basePath != "")
            {
                string languagePath = basePath + Path.DirectorySeparatorChar + "Locale";

                if (Directory.Exists(languagePath))
                {
                    string[] languageFiles = Directory.GetFiles(languagePath, "xml");

                    foreach (string languageFile in languageFiles)
                    {
                        StreamReader reader = new StreamReader(languageFile);
                        Language loadedLanguage = DeserialiseLanguage(reader);

                        if(loadedLanguage != null)
                        {
                            _languages.Add(loadedLanguage);
                        }
                    }
                }
                else
                {
                    _toolBase.DetailedLogger.LogWarning("Can't load any localisation files");
                }
            }
        }

        protected Language DeserialiseLanguage(TextReader reader)
        {
            XmlSerializer xmlSerialiser = new XmlSerializer(typeof(Language));

            Language loadedLanguage = (Language)xmlSerialiser.Deserialize(reader);
            reader.Close();

            return loadedLanguage;
        }

        /// <summary>
        /// Returns a list of languages which are available to the mod. This will return readable languages for use on the UI
        /// </summary>
        /// <returns>A list of languages available.</returns>
        public List<string> AvailableLanguagesReadable()
        {
            List<string> languageNames = new List<string>();

            foreach(Language availableLanguage in _languages)
            {
                languageNames.Add(availableLanguage._readableName);
            }

            return languageNames;
        }

        /// <summary>
        /// Returns a list of languages which are available to the mod. This will return language IDs for searching.
        /// </summary>
        /// <returns>A list of languages available.</returns>
        public List<string> AvailableLanguages()
        {
            List<string> languageNames = new List<string>();

            foreach (Language availableLanguage in _languages)
            {
                languageNames.Add(availableLanguage._uniqueName);
            }

            return languageNames;
        }

        /// <summary>
        /// Translate all text into the specified language
        /// </summary>
        /// <param name="languageID">The unique language name to translate to</param>
        public bool TranslateTo(string languageID)
        {
            bool success = false;

            foreach(Language availableLanguage in _languages)
            {
                if(availableLanguage._uniqueName == languageID)
                {
                    List<FieldInfo> translatableFields = GetTranslatableFields(_toolBase.ModSettings.ModAssembly);

                    foreach (FieldInfo translatableField in translatableFields)
                    {
                        object[] foundAttributes = translatableField.GetCustomAttributes(typeof(TranslatableAttribute), false);

                        if (foundAttributes.Length > 0)
                        {
                            TranslatableAttribute translatableInfo = foundAttributes[0] as TranslatableAttribute;
                            string identifier = translatableInfo.identifier;

                            if(availableLanguage._conversionDictionary.ContainsKey(identifier))
                            {
                                translatableField.SetValue(null, availableLanguage._conversionDictionary[identifier]);
                            }
                            else
                            {
                                translatableField.SetValue(null, identifier);
                                _toolBase.DetailedLogger.LogWarning("Translation language \"" + languageID + "\" doesn't contain a suitable translation for \"" + identifier + "\"");
                            }
                        }
                    }

                    if (OnLanguageChanged != null)
                    {
                        OnLanguageChanged(languageID);
                    }
                    success = true;

                    break;
                }
            }

            return success;
        }
    }

    [XmlRoot(ElementName = "Language", Namespace = "", IsNullable = false)]
    public class Language
    {
        [XmlAttribute("UniqueName")]
        public string _uniqueName = "";

        [XmlAttribute("ReadableName")]
        public string _readableName = "";

        [XmlArray("Translations", IsNullable = false)]
        [XmlArrayItem("Translation", IsNullable = false)]
        public LanguageConversion[] _conversionGroups
        {
            get
            {
                LanguageConversion[] returnArray = new LanguageConversion[_conversionDictionary.Count];

                int index = 0;
                foreach(KeyValuePair<string, string> conversion in _conversionDictionary)
                {
                    returnArray[index] = new LanguageConversion() { _key = conversion.Key, _value = conversion.Value };
                    ++index;
                }

                return returnArray;
            }

            set
            {
                foreach(LanguageConversion conversion in value)
                {
                    _conversionDictionary[conversion._key] = conversion._value;
                }
            }
        }

        [XmlIgnore]
        public Dictionary<string, string> _conversionDictionary = new Dictionary<string, string>();
    }

    public class LanguageConversion
    {
        [XmlElement("ID", IsNullable = false)]
        public string _key = "";

        [XmlElement("String", IsNullable = false)]
        public string _value = "";
    }
}
