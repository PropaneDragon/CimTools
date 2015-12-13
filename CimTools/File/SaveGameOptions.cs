using CimTools.File;
using ICities;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CimTools.Utilities
{
    class SaveGameOptions : SerializableDataExtensionBase
    {
        public override void OnSaveData()
        {
            if (Settings.ModName != null)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();

                SavedGroup[] groups = InternalOptions.Instance().SavedData.ToArray();

                if(groups != null)
                {
                    binaryFormatter.Serialize(memoryStream, groups);
                    serializableDataManager.SaveData(Settings.ModName + "Data", memoryStream.ToArray());
                }
            }
        }

        public override void OnLoadData()
        {
            if(Settings.ModName != null)
            {
                byte[] deserialisedData = serializableDataManager.LoadData(Settings.ModName + "Data");

                if(deserialisedData != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    memoryStream.Write(deserialisedData, 0, deserialisedData.Length);
                    memoryStream.Position = 0;

                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    try
                    {
                        SavedGroup[] groups = binaryFormatter.Deserialize(memoryStream) as SavedGroup[];

                        if(groups != null)
                        {
                            InternalOptions.Instance().SavedData = groups.ToList();
                        }
                    }
                    catch
                    {

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
