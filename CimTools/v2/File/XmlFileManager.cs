using CimTools.v2.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace CimTools.v2.File
{
    internal class ClassData
    {
        public string outputName = null;
        public object instance = null;
        public FieldInfo[] fields = null;
    }

    public class XmlFileManager
    {        
        protected CimToolBase m_toolBase;
        protected List<object> m_nonStaticObjectsToSave = new List<object>();
        protected XmlOptionsAttribute.OptionType m_optionType = XmlOptionsAttribute.OptionType.XmlFile;

        /// <summary>
        /// Handles IO between classes and the options XML file.
        /// </summary>
        /// <param name="toolBase"></param>
        public XmlFileManager(CimToolBase toolBase, XmlOptionsAttribute.OptionType optionType = XmlOptionsAttribute.OptionType.XmlFile)
        {
            m_toolBase = toolBase;
            m_optionType = optionType;
        }

        /// <summary>
        /// Automatically handles saving and loading of variables to
        /// XML for this object.
        /// </summary>
        /// <param name="objectToSave">The object you wish to save to XML</param>
        public void AddObjectToSave(object objectToSave)
        {
            m_nonStaticObjectsToSave.Add(objectToSave);
        }

        /// <summary>
        /// Save an XML document to the default location
        /// </summary>
        public bool Save()
        {
            StreamWriter writer = new StreamWriter(m_toolBase.ModSettings.ModName + "Options.xml");

            return Save(writer);
        }

        /// <summary>
        /// Save an XML document to a custom writer
        /// </summary>
        /// <param name="writer">The writer to save the XML document to</param>
        public bool Save(TextWriter writer)
        {
            List<ClassData> hierarchies = GetHierarchy();
            XmlWriter xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings() { NewLineOnAttributes = true, Indent = true });

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Settings");

            foreach (ClassData element in hierarchies)
            {
                xmlWriter.WriteStartElement(element.outputName);
                WriteInternalElement(xmlWriter, element);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();

            writer.Close();

            return true; //Should be temporary until I can think of something that can cause this to fail
        }

        public void Load()
        {
            if (System.IO.File.Exists(m_toolBase.ModSettings.ModName + "Options.xml"))
            {
                StreamReader reader = new StreamReader(m_toolBase.ModSettings.ModName + "Options.xml");
                Load(reader);
            }
        }

        public void Load(TextReader reader)
        {
            List<ClassData> hierarchies = GetHierarchy();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(reader);

            XmlNodeList settingsElement = xmlDocument.GetElementsByTagName("Settings");

            foreach (XmlNode element in settingsElement[0])
            {
                foreach(ClassData hierarchyData in hierarchies)
                {
                    if(hierarchyData.outputName == element.Name)
                    {
                        foreach (FieldInfo field in hierarchyData.fields)
                        {
                            XmlNode foundNode = null;

                            foreach (XmlNode innerElement in element.ChildNodes)
                            {
                                if (innerElement.Name == field.Name)
                                {
                                    foundNode = innerElement;
                                    break;
                                }
                            }

                            if (foundNode != null)
                            {
                                Type elementType = field.FieldType;

                                if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                                {
                                    Type[] dictionaryTypes = elementType.GetGenericArguments();
                                    Type keyType = dictionaryTypes[0];
                                    Type valueType = dictionaryTypes[1];

                                    IDictionary dictionary = field.GetValue(hierarchyData.instance) as IDictionary;
                                    dictionary.Clear();

                                    foreach (XmlNode innerElement in foundNode.ChildNodes)
                                    {
                                        try
                                        {
                                            dictionary.Add(innerElement.Name.ToString(), Convert.ChangeType(innerElement.InnerText, valueType));
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                                else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(List<>))
                                {
                                    Type[] dictionaryTypes = elementType.GetGenericArguments();
                                    Type valueType = dictionaryTypes[0];

                                    IList list = field.GetValue(hierarchyData.instance) as IList;
                                    list.Clear();

                                    foreach (XmlNode innerElement in foundNode.ChildNodes)
                                    {

                                        try
                                        {
                                            list.Add(Convert.ChangeType(innerElement.InnerText, valueType));
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        field.SetValue(hierarchyData.instance, Convert.ChangeType(foundNode.InnerText, field.FieldType));
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }

            reader.Close();
        }

        private List<ClassData> GetHierarchy()
        {
            List<ClassData> returnList = new List<ClassData>();

            m_toolBase.DetailedLogger.Log("Finding hierarchies for XML data with type " + m_optionType.ToString() + "...");

            foreach (Assembly assembly in m_toolBase.ModSettings.Assemblies)
            {
                m_toolBase.DetailedLogger.Log("Searching " + assembly.FullName);

                foreach (Type individualType in assembly.GetTypes())
                {
                    AddClassToList(individualType, null, ref returnList);
                }
            }

            foreach(object nonStaticObject in m_nonStaticObjectsToSave)
            {
                AddClassToList(nonStaticObject.GetType(), nonStaticObject, ref returnList);
            }

            return returnList;
        }

        private void AddClassToList(Type classType, object classObject, ref List<ClassData> referenceList)
        {
            object[] FoundXmlAttributes = classType.GetCustomAttributes(typeof(XmlOptionsAttribute), false);

            if (FoundXmlAttributes.Length != 0)
            {
                XmlOptionsAttribute thisAttribute = FoundXmlAttributes[0] as XmlOptionsAttribute;
                Dictionary<string, object> individualClass = new Dictionary<string, object>();
                FieldInfo[] fields = classType.GetFields(BindingFlags.Public | (classObject == null ? BindingFlags.Static : BindingFlags.Instance));
                string savedName = classType.Name;

                if(fields.Length > 0 && thisAttribute.type == m_optionType)
                {
                    m_toolBase.DetailedLogger.Log("Found type " + classType.Name + " with XmlOptionsAttribute from object " + (classObject == null ? "null" : classObject.GetType().ToString()));

                    if (thisAttribute != null && thisAttribute.key != null)
                    {
                        savedName = thisAttribute.key;
                    }

                    referenceList.Add(new ClassData() { fields = fields, instance = classObject, outputName = savedName });
                }
            }
        }

        private void WriteInternalElement(XmlWriter writer, ClassData classInfo)
        {
            foreach (FieldInfo field in classInfo.fields)
            {
                Type elementType = field.FieldType;

                if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    m_toolBase.DetailedLogger.Log("Writing out dictionary");

                    Type[] dictionaryTypes = elementType.GetGenericArguments();
                    Type keyType = dictionaryTypes[0];

                    if (keyType == typeof(string))
                    {
                        writer.WriteStartElement(field.Name);
                        WriteInternalElement(writer, GetObjectDictionary(field.GetValue(classInfo.instance) as IDictionary));
                        writer.WriteEndElement();
                    }
                }
                else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    m_toolBase.DetailedLogger.Log("Writing out list");
                    writer.WriteStartElement(field.Name);
                    WriteInternalElement(writer, GetObjectList(field.GetValue(classInfo.instance) as IList));
                    writer.WriteEndElement();
                }
                else
                {
                    m_toolBase.DetailedLogger.Log("Writing out element \"" + field.Name + "\" with value " + field.GetValue(classInfo.instance));
                    writer.WriteElementString(field.Name, Convert.ChangeType(field.GetValue(classInfo.instance), typeof(string)) as string);
                }
            }
        }

        private void WriteInternalElement(XmlWriter writer, Dictionary<string, object> extractionData)
        {
            foreach (KeyValuePair<string, object> field in extractionData)
            {
                Type elementType = field.Value.GetType();

                if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    Type[] dictionaryTypes = elementType.GetGenericArguments();
                    Type keyType = dictionaryTypes[0];
                    Type valueType = dictionaryTypes[1];

                    if (keyType == typeof(string))
                    {
                        writer.WriteStartElement(field.Key);
                        WriteInternalElement(writer, GetObjectDictionary(field.Value as IDictionary));
                        writer.WriteEndElement();
                    }
                }
                else
                {
                    m_toolBase.DetailedLogger.Log("Writing out element \"" + field.Key + "\" with value " + field.Value);
                    writer.WriteElementString(field.Key, Convert.ChangeType(field.Value, typeof(string)) as string);
                }
            }
        }

        private void WriteInternalElement(XmlWriter writer, List<object> extractionData)
        {
            foreach (object field in extractionData)
            {
                Type elementType = field.GetType();

                if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(List<>))
                {                    
                    writer.WriteStartElement("Item");
                    WriteInternalElement(writer, GetObjectList(field as IList));
                    writer.WriteEndElement();
                }
                else
                {
                    m_toolBase.DetailedLogger.Log("Writing out element " + field);
                    writer.WriteElementString("Item", Convert.ChangeType(field, typeof(string)) as string);
                }
            }
        }

        private Dictionary<string, object> GetObjectDictionary(IDictionary dictionary)
        {
            Dictionary<string, object> castedDictionary = new Dictionary<string, object>();

            foreach (DictionaryEntry entry in dictionary)
            {
                castedDictionary.Add(entry.Key as string, entry.Value);
            }

            return castedDictionary;
        }

        private List<object> GetObjectList(IList list)
        {
            List<object> castedList = new List<object>();

            foreach (object entry in list)
            {
                castedList.Add(entry);
            }

            return castedList;
        }
    }
}
