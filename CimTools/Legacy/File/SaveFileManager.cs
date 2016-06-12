using ICities;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CimTools.Legacy.File
{
    [Obsolete("This has been replaced in v2", false)]
    public class SaveFileManager
    {
        private CimToolSettings _modSettings = null;
        private XmlFileManager _xmlManager = null;

        public XmlFileOptions Data
        {
            get { return _xmlManager.Data; }
        }

        [Obsolete("This has been replaced in v2", false)]
        public SaveFileManager(CimToolSettings modSettings)
        {
            _modSettings = modSettings;
            _xmlManager = new XmlFileManager(modSettings);
        }

        [Obsolete("This has been replaced in v2", false)]
        public void LoadData(ISerializableData serialisableDataManager)
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
