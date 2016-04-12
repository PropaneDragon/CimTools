using CimTools.v2.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

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
        internal CimToolSettings m_modSettings;
        internal List<object> m_nonStaticObjectsToSave = new List<object>();

        /// <summary>
        /// Handles IO between classes and the options XML file.
        /// </summary>
        /// <param name="modSettings"></param>
        public XmlFileManager(CimToolSettings modSettings)
        {
            m_modSettings = modSettings;
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
        public void Save()
        {
            StreamWriter writer = new StreamWriter(m_modSettings.ModName + "Options.xml");

            Save(writer);
        }

        /// <summary>
        /// Save an XML document to a custom writer
        /// </summary>
        /// <param name="writer">The writer to save the XML document to</param>
        public void Save(TextWriter writer)
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
        }

        public void Load()
        {
            if (System.IO.File.Exists(m_modSettings.ModName + "Options.xml"))
            {
                StreamReader reader = new StreamReader(m_modSettings.ModName + "Options.xml");
                Load(reader);
            }
        }

        public void Load(TextReader reader)
        {
            List<ClassData> hierarchies = GetHierarchy();
            XDocument xmlDocument = XDocument.Load(XmlReader.Create(reader));
            XElement settingsElement = xmlDocument.Element("Settings");

            foreach (XElement element in settingsElement.Descendants())
            {
                foreach(ClassData hierarchyData in hierarchies)
                {
                    if(hierarchyData.outputName == element.Name)
                    {
                        foreach (FieldInfo field in hierarchyData.fields)
                        {
                            foreach(XElement innerElement in element.Descendants(field.Name))
                            {
                                Type elementType = field.FieldType;

                                if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                                {
                                    Type[] dictionaryTypes = elementType.GetGenericArguments();
                                    Type keyType = dictionaryTypes[0];
                                    Type valueType = dictionaryTypes[1];

                                    IDictionary dictionary = field.GetValue(hierarchyData.instance) as IDictionary;
                                    dictionary.Clear();

                                    foreach (XElement keyValuePair in innerElement.Descendants())
                                    {
                                        try
                                        {
                                            dictionary.Add(keyValuePair.Name.ToString(), Convert.ChangeType(keyValuePair.Value, valueType));
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

                                    foreach (XElement keyValuePair in innerElement.Descendants())
                                    {
                                        try
                                        {
                                            list.Add(Convert.ChangeType(keyValuePair.Value, valueType));
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
                                        field.SetValue(hierarchyData.instance, Convert.ChangeType(innerElement.Value, field.FieldType));
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
        }

        private List<ClassData> GetHierarchy()
        {
            List<ClassData> returnList = new List<ClassData>();
            
            foreach (Type individualType in m_modSettings.ModAssembly.GetTypes())
            {
                AddClassToList(individualType, null, ref returnList);
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

                if(fields.Length > 0)
                {
                    if (thisAttribute != null && thisAttribute.Key != null)
                    {
                        savedName = thisAttribute.Key;
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
                    Type[] dictionaryTypes = elementType.GetGenericArguments();
                    Type keyType = dictionaryTypes[0];
                    Type valueType = dictionaryTypes[1];

                    if (keyType == typeof(string))
                    {
                        writer.WriteStartElement(field.Name);
                        WriteInternalElement(writer, GetObjectDictionary(field.GetValue(classInfo.instance) as IDictionary));
                        writer.WriteEndElement();
                    }
                }
                else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    writer.WriteStartElement(field.Name);
                    WriteInternalElement(writer, GetObjectList(field.GetValue(classInfo.instance) as IList));
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteElementString(field.Name, field.GetValue(classInfo.instance).ToString());
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
                    writer.WriteElementString(field.Key, field.Value.ToString());
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
                    writer.WriteElementString("Item", field.ToString());
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
