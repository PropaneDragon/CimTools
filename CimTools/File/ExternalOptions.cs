using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CimTools.File
{
    /// <summary>
    /// Handles saving and loading options from the disk. These options are
    /// saved to the same folder as the game executable and are not saved
    /// inside the save file for the level. You should use SaveFileOptions
    /// for that.
    /// </summary>
    [Serializable()]
    [XmlRoot(ElementName = "SavedOptions")]
    public class ExternalOptions
    {
        /// <summary>
        /// A list of sliders to save
        /// </summary>
        public List<SavedSlider> SavedSliders = new List<SavedSlider>();

        /// <summary>
        /// A list of checkboxes to save
        /// </summary>
        public List<SavedCheckbox> SavedCheckboxes = new List<SavedCheckbox>();

        /// <summary>
        /// A list of dropdowns to save
        /// </summary>
        public List<SavedDropdown> SavedDropdowns = new List<SavedDropdown>();

        private static ExternalOptions instance = null;

        /// <summary>
        /// Gets an instance of the SavedOptions
        /// </summary>
        /// <returns>An instance of SavedOptions</returns>
        public static ExternalOptions Instance()
        {
            if (instance == null)
            {
                instance = new ExternalOptions();
            }

            return instance;
        }

        /// <summary>
        /// Change the instance used for the options.
        /// </summary>
        /// <param name="optionManager">The SavedOptions to replace the existing instance</param>
        public static void SetInstance(ExternalOptions optionManager)
        {
            if (optionManager != null)
            {
                instance = optionManager;
            }
        }
        
        /// <summary>
        /// Saves all options to the disk. Make sure you've updated the options first.
        /// </summary>
        public static void Save(string name)
        {
            XmlSerializer xmlSerialiser = new XmlSerializer(typeof(ExternalOptions));
            StreamWriter writer = new StreamWriter(name + ".xml");

            xmlSerialiser.Serialize(writer, Instance());
            writer.Close();
        }

        /// <summary>
        /// Load all options from the disk.
        /// </summary>
        /// <returns>Whether loading was successful</returns>
        public static bool Load(string name)
        {
            bool success = false;

            if (System.IO.File.Exists(name + ".xml"))
            {
                XmlSerializer xmlSerialiser = new XmlSerializer(typeof(ExternalOptions));
                StreamReader reader = new StreamReader(name + ".xml");

                ExternalOptions savedOptions = xmlSerialiser.Deserialize(reader) as ExternalOptions;
                reader.Close();

                if (savedOptions != null)
                {
                    SetInstance(savedOptions);
                    success = true;
                }
            }

            return success;
        }
    }

    /// <summary>
    /// Container for saving data for a slider
    /// </summary>
    [Serializable()]
    public class SavedSlider
    {
        /// <summary>
        /// Slider name
        /// </summary>
        [XmlElement("Name", IsNullable = false)]
        public string name = "";

        /// <summary>
        /// Value of the slider
        /// </summary>
        [XmlElement("Value", IsNullable = false)]
        public float value = 0f;
    }

    /// <summary>
    /// Container for saving data for a checkbox
    /// </summary>
    [Serializable()]
    public class SavedCheckbox
    {
        /// <summary>
        /// Checkbox name
        /// </summary>
        [XmlElement("Name", IsNullable = false)]
        public string name = "";

        /// <summary>
        /// Value of the checkbox
        /// </summary>
        [XmlElement("Value", IsNullable = false)]
        public bool value = false;
    }

    /// <summary>
    /// Container for saving data for a dropdown
    /// </summary>
    [Serializable()]
    public class SavedDropdown
    {
        /// <summary>
        /// Dropdown name
        /// </summary>
        [XmlElement("Name", IsNullable = false)]
        public string name = "";

        /// <summary>
        /// Value of the dropdown
        /// </summary>
        [XmlElement("Value", IsNullable = false)]
        public string value = "";
    }
}
