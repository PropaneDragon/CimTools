using CimTools.V1.File;
using ICities;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace CimTools.V1.Utilities
{
    public class SaveFileManager
    {
        private CimToolSettings _modSettings = null;
        private XmlFileManager _xmlManager = null;

        public XmlFileOptions Data
        {
            get { return _xmlManager.Data; }
        }

        public SaveFileManager(CimToolSettings modSettings)
        {
            _modSettings = modSettings;
            _xmlManager = new XmlFileManager(modSettings);
        }

        public void OnSaveData(ISerializableData serialisableDataManager)
        {
            if (_modSettings.ModName != null)
            {                
                StringWriter stringWriter = new StringWriter();

                if (_xmlManager.Save(stringWriter) == ExportOptionBase.OptionError.NoError)
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    MemoryStream memoryStream = new MemoryStream();

                    Debug.Log(stringWriter.ToString());

                    binaryFormatter.Serialize(memoryStream, stringWriter.ToString());
                    serialisableDataManager.SaveData(_modSettings.ModName + "Data", memoryStream.ToArray());

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
            if(_modSettings.ModName != null)
            {
                byte[] deserialisedData = serialisableDataManager.LoadData(_modSettings.ModName + "Data");

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
