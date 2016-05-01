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
        private XmlFileManager _xmlManager = null;

        public SaveFileManager(CimToolBase toolBase)
        {
            _toolBase = toolBase;
            _xmlManager = new XmlFileManager(toolBase);
        }

        public void OnSaveData(ISerializableData serialisableDataManager)
        {
            if (_toolBase.ModSettings.ModName != null)
            {                
                StringWriter stringWriter = new StringWriter();

                if (_xmlManager.Save(stringWriter))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    MemoryStream memoryStream = new MemoryStream();

                    Debug.Log(stringWriter.ToString());

                    binaryFormatter.Serialize(memoryStream, stringWriter.ToString());
                    serialisableDataManager.SaveData(_toolBase.ModSettings.ModName + "Data", memoryStream.ToArray());

                    memoryStream.Close();
                }
                else
                {
                    Debug.LogError("Could not save data.");
                }
            }
        }

        public void OnLoadData(ISerializableData serialisableDataManager)
        {
            if(_toolBase.ModSettings.ModName != null)
            {
                byte[] deserialisedData = serialisableDataManager.LoadData(_toolBase.ModSettings.ModName + "Data");

                if(deserialisedData != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    memoryStream.Write(deserialisedData, 0, deserialisedData.Length);
                    memoryStream.Position = 0;

                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    try
                    {
                        string xmlData = binaryFormatter.Deserialize(memoryStream) as string;

                        if(xmlData != null)
                        {
                            StringReader stringReader = new StringReader(xmlData);
                            _xmlManager.Load(stringReader);
                        }
                    }
                    catch(Exception ex)
                    {
                        Debug.Log("Failed to load options");
                        Debug.LogException(ex);
                    }
                    finally
                    {
                        memoryStream.Close();
                    }
                }
            }
        }
    }
}
