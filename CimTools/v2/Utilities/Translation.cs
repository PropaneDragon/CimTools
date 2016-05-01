using CimTools.v2.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace CimTools.v2.Utilities
{
    public delegate void LanguageChangedEventHandler(string languageIdentifier);

    /// <summary>
    /// Handles localisation for a mod.
    /// </summary>
    public class Translation
    {
        public event LanguageChangedEventHandler OnLanguageChanged;

        protected CimToolBase _toolBase = null;
        protected List<Language> _languages = new List<Language>();
        protected Dictionary<Assembly, List<FieldInfo>> _fields = new Dictionary<Assembly, List<FieldInfo>>();
        protected Language _currentLanguage = null;
        protected bool _languagesLoaded = false;
        protected bool _loadLanguageAutomatically = true;

        public Translation(CimToolBase toolBase, bool loadLanguageAutomatically = true)
        {
            _toolBase = toolBase;
            _loadLanguageAutomatically = loadLanguageAutomatically;
        }

        /// <summary>
        /// Generates a language template for the mod
        /// </summary>
        public void GenerateLanguageTemplate()
        {
            List<FieldInfo> translatableFields = GetTranslatableFields(_toolBase.ModSettings.MainAssembly);
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
            if (!_fields.ContainsKey(containingAssembly))
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

                _fields[containingAssembly] = translatableFields;
                return translatableFields;
            }
            else
            {
                return _fields[containingAssembly];
            }
        }

        public void LoadLanguages()
        {
            if (!_languagesLoaded && _loadLanguageAutomatically)
            {
                _languagesLoaded = true;
                RefreshLanguages();
                _currentLanguage = _languages[0];
            }
        }

        /// <summary>
        /// Loads all languages from the Locale folder
        /// </summary>
        public void RefreshLanguages()
        {
            _languages.Clear();

            string basePath = _toolBase.Path.GetModPath();

            if (basePath != "")
            {
                string languagePath = basePath + Path.DirectorySeparatorChar + "Locale";

                if (Directory.Exists(languagePath))
                {
                    string[] languageFiles = Directory.GetFiles(languagePath);

                    foreach (string languageFile in languageFiles)
                    {
                        StreamReader reader = new StreamReader(languageFile);
                        Language loadedLanguage = DeserialiseLanguage(reader);

                        if (loadedLanguage != null)
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
            LoadLanguages();

            List<string> languageNames = new List<string>();

            foreach (Language availableLanguage in _languages)
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
            LoadLanguages();

            List<string> languageNames = new List<string>();

            foreach (Language availableLanguage in _languages)
            {
                languageNames.Add(availableLanguage._uniqueName);
            }

            return languageNames;
        }

        /// <summary>
        /// Returns a list of Language unique IDs that have the name
        /// </summary>
        /// <param name="name">The name of the language to get IDs for</param>
        /// <returns>A list of IDs that match</returns>
        public List<string> GetLanguageIDsFromName(string name)
        {
            List<string> returnLanguages = new List<string>();

            foreach (Language availableLanguage in _languages)
            {
                if (availableLanguage._readableName == name)
                {
                    returnLanguages.Add(availableLanguage._uniqueName);
                }
            }

            return returnLanguages;
        }

        /// <summary>
        /// Translate all text into the specified language
        /// </summary>
        /// <param name="languageID">The unique language name to translate to</param>
        public bool TranslateTo(string languageID)
        {
            LoadLanguages();

            bool success = false;

            foreach (Language availableLanguage in _languages)
            {
                if (availableLanguage._uniqueName == languageID)
                {
                    _currentLanguage = availableLanguage;
                    List<FieldInfo> translatableFields = GetTranslatableFields(_toolBase.ModSettings.MainAssembly);

                    foreach (FieldInfo translatableField in translatableFields)
                    {
                        object[] foundAttributes = translatableField.GetCustomAttributes(typeof(TranslatableAttribute), false);

                        if (foundAttributes.Length > 0)
                        {
                            TranslatableAttribute translatableInfo = foundAttributes[0] as TranslatableAttribute;
                            string identifier = translatableInfo.identifier;

                            if (_currentLanguage._conversionDictionary.ContainsKey(identifier))
                            {
                                translatableField.SetValue(null, _currentLanguage._conversionDictionary[identifier]);
                                _toolBase.DetailedLogger.Log("Translation language \"" + languageID + "\" is returning \"" + _currentLanguage._conversionDictionary[identifier] + "\" for \"" + identifier + "\"");
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
                        Delegate[] invocationList = OnLanguageChanged.GetInvocationList();

                        _toolBase.DetailedLogger.Log("Invocation list size: " + invocationList.Length.ToString());

                        int count = 0;

                        foreach (Delegate method in invocationList)
                        {
                            _toolBase.DetailedLogger.Log("Event #" + (++count).ToString());
                            _toolBase.DetailedLogger.Log("Invoking language change on " + method.Method.Name);
                            method.DynamicInvoke(languageID);
                        }
                    }

                    success = true;

                    break;
                }
            }

            return success;
        }

        public bool HasTranslation(string translationId)
        {
            LoadLanguages();

            return _currentLanguage != null && _currentLanguage._conversionDictionary.ContainsKey(translationId);
        }

        public string GetTranslation(string translationId)
        {
            LoadLanguages();

            string translatedText = translationId;

            if (_currentLanguage != null)
            {
                if (HasTranslation(translationId))
                {
                    translatedText = _currentLanguage._conversionDictionary[translationId];
                    _toolBase.DetailedLogger.Log("Returned translation for language \"" + _currentLanguage._uniqueName + "\" is returning \"" + translatedText + "\" for \"" + translationId + "\"");
                }
                else
                {
                    _toolBase.DetailedLogger.LogWarning("Returned translation for language \"" + _currentLanguage._uniqueName + "\" doesn't contain a suitable translation for \"" + translationId + "\"");
                }
            }
            else
            {
                _toolBase.DetailedLogger.LogWarning("Can't get a translation for \"" + translationId + "\" as there is not a language defined");
            }

            return translatedText;
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
                foreach (KeyValuePair<string, string> conversion in _conversionDictionary)
                {
                    returnArray[index] = new LanguageConversion() { _key = conversion.Key, _value = conversion.Value };
                    ++index;
                }

                return returnArray;
            }

            set
            {
                foreach (LanguageConversion conversion in value)
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
        [XmlAttribute("ID")]
        public string _key = "";

        [XmlAttribute("String")]
        public string _value = "";
    }
}
