using CimTools.v2.File;
using ICities;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CimTools.v2.Utilities
{
    public class SaveFileManager
    {
        private CimToolBase _toolBase = null;

        public XmlFileManager _xmlManager = null;

        public SaveFileManager(CimToolBase toolBase)
        {
            _toolBase = toolBase;
            _xmlManager = new XmlFileManager(toolBase, Attributes.XmlOptionsAttribute.OptionType.SaveFile);
        }

        /// <summary>
        /// When the game is saved. Connect this up to the OnSaveData method in your mod which
        /// should be inherited from SerializableDataExtensionBase.
        /// </summary>
        /// <param name="serialisableDataManager">The data manager passed from SerializableDataExtensionBase</param>
        public void SaveData(ISerializableData serialisableDataManager)
        {
            if (serialisableDataManager != null)
            {
                if (_toolBase.ModSettings.ModName != null)
                {
                    StringWriter stringWriter = new StringWriter();

                    if (_xmlManager.Save(stringWriter))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        MemoryStream memoryStream = new MemoryStream();
                        
                        _toolBase.NamedLogger.Log("Saving data to save file.");
                        _toolBase.DetailedLogger.Log(stringWriter.ToString());

                        binaryFormatter.Serialize(memoryStream, stringWriter.ToString());
                        serialisableDataManager.SaveData(_toolBase.ModSettings.ModName + "Data" + _toolBase.Strings.VERSION, memoryStream.ToArray());

                        _toolBase.NamedLogger.Log("Saved.");

                        memoryStream.Close();
                    }
                    else
                    {
                        _toolBase.NamedLogger.LogError("Failed to save options");
                    }
                }
            }
            else
            {
                _toolBase.NamedLogger.LogError("The serialisableDataManager passed to SaveData was null. No data was saved!");
            }
        }

        public void LoadData(ISerializableData serialisableDataManager)
        {
            if (serialisableDataManager != null)
            {
                if (_toolBase.ModSettings.ModName != null)
                {
                    byte[] deserialisedData = serialisableDataManager.LoadData(_toolBase.ModSettings.ModName + "Data" + _toolBase.Strings.VERSION);

                    if (deserialisedData != null)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        memoryStream.Write(deserialisedData, 0, deserialisedData.Length);
                        memoryStream.Position = 0;

                        BinaryFormatter binaryFormatter = new BinaryFormatter();

                        try
                        {
                            string xmlData = binaryFormatter.Deserialize(memoryStream) as string;

                            if (xmlData != null)
                            {
                                StringReader stringReader = new StringReader(xmlData);

                                _toolBase.NamedLogger.Log("Loading data from save file.");

                                _xmlManager.Load(stringReader);

                                _toolBase.DetailedLogger.Log(stringReader.ToString());
                                _toolBase.NamedLogger.Log("Loaded.");
                            }
                        }
                        catch (Exception ex)
                        {
                            _toolBase.NamedLogger.LogError("Failed to load options");
                            Debug.LogException(ex);
                        }
                        finally
                        {
                            memoryStream.Close();
                        }
                    }
                }
            }
            else
            {
                _toolBase.NamedLogger.LogError("The serialisableDataManager passed to LoadData was null. No data was loaded!");
            }
        }
    }
}
