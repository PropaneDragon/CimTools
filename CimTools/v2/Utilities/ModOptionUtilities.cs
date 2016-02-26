using CimTools.v2.File;
using ColossalFramework.UI;
using ICities;
using System.Collections.Generic;
using UnityEngine;

namespace CimTools.v2.Utilities
{
    /// <summary>
    /// When the user has saved the options
    /// </summary>
    public delegate void OptionPanelSaved();

    /// <summary>
    /// Handles options on the mod option panel ingame
    /// </summary>
    public class ModOptionUtilities
    {
        private CimToolBase _mToolBase = null;
        private List<OptionsItemBase> _options = new List<OptionsItemBase>();

        /// <summary>
        /// When the options have been saved by the user.
        /// </summary>
        public event OptionPanelSaved OnOptionPanelSaved;

        public ModOptionUtilities(CimToolBase toolBase)
        {
            _mToolBase = toolBase;
        }

        /// <summary>
        /// Creates options on a panel using the helper
        /// </summary>
        /// <param name="helper">The UIHelper to put the options on</param>
        /// <param name="groupName">The title of the group in the options panel.</param>
        public void CreateOptions(UIHelperBase helper, List<OptionsItemBase> options, string groupName = "Options")
        {
            _options.AddRange(options);
            LoadOptions();

            UIHelperBase optionGroup = helper.AddGroup(groupName);

            foreach(OptionsItemBase option in options)
            {
                option.Create(optionGroup);
            }

            UIButton saveButton = optionGroup.AddButton("Apply", SaveOptions) as UIButton;
            saveButton.width = 120;
            saveButton.color = new Color32(0, 255, 0, 255);
        }

        /// <summary>
        /// Manually save the UI options to the persistent options. When this is called
        /// by the class internally, you can get the event using OnOptionPanelSaved()
        /// </summary>
        public void SaveOptions()
        {
            /*foreach(OptionsItemBase option in _options)
            {
                _mToolBase.XMLFileOptions.Data.SetValue(option.uniqueName, option.m_value, "IngameOptions");
            }

            if (OnOptionPanelSaved != null)
            {
                OnOptionPanelSaved();
            }

            _mToolBase.XMLFileOptions.Save();*/
        }

        /// <summary>
        /// Manually load the UI options onto the existing panel elements.
        /// </summary>
        public void LoadOptions()
        {
            /*_mToolBase.XMLFileOptions.Load();

            foreach (OptionsItemBase option in _options)
            {
                object foundValue = null;

                if(_mToolBase.XMLFileOptions.Data.GetValue(option.uniqueName, ref foundValue, "IngameOptions", false) == ExportOptionBase.OptionError.NoError)
                {
                    option.m_value = foundValue;
                }
            }*/
        }

        public bool GetOptionValue<T>(string uniqueName, ref T value)
        {
            bool found = false;

            foreach (OptionsItemBase option in _options)
            {
                if(option.uniqueName == uniqueName)
                {
                    value = (T)option.m_value;
                    found = true;

                    break;
                }
            }

            return found;
        }
    }

    /// <summary>
    /// Base class for all option panel options.
    /// </summary>
    public abstract class OptionsItemBase
    {
        public object m_value = default(object);

        /// <summary>
        /// The unique option name. Can't clash with any other option names
        /// or you'll lose data.
        /// </summary>
        public string uniqueName = "";

        /// <summary>
        /// The name that appears on the UI.
        /// </summary>
        public string readableName = "";

        /// <summary>
        /// Whether the option is enabled or not
        /// </summary>
        public bool enabled = true;

        /// <summary>
        /// Create the element on the helper
        /// </summary>
        /// <param name="helper">The UIHelper to attach the element to</param>
        public abstract void Create(UIHelperBase helper);

        public void IgnoredFunction<T>(T ignored) { }
    }

    /// <summary>
    /// Checkbox option
    /// </summary>
    public class OptionsCheckbox : OptionsItemBase
    {
        /// <summary>
        /// The default value of the object, or the saved value if loaded.
        /// </summary>
        public bool value
        {
            get { return (bool)m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// Create the element on the helper
        /// </summary>
        /// <param name="helper">The UIHelper to attach the element to</param>
        public override void Create(UIHelperBase helper)
        {
            UICheckBox checkBox = helper.AddCheckbox(readableName, value, IgnoredFunction) as UICheckBox;
            checkBox.readOnly = !enabled;
            checkBox.name = uniqueName;
            checkBox.disabledColor = new Color32(100, 100, 100, 255);
            checkBox.eventCheckChanged += new PropertyChangedEventHandler<bool>(delegate (UIComponent component, bool newValue)
            {
                value = newValue;
            });
        }
    }

    /// <summary>
    /// Slider option
    /// </summary>
    public class OptionsSlider : OptionsItemBase
    {
        /// <summary>
        /// The default value of the object, or the saved value if loaded.
        /// </summary>
        public float value
        {
            get { return (float)m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// Lower bound for the slider
        /// </summary>
        public float min = 0f;

        /// <summary>
        /// Upper bound for the slider
        /// </summary>
        public float max = 1f;

        /// <summary>
        /// The amount to step when the user slides the slider
        /// </summary>
        public float step = 1f;

        /// <summary>
        /// Create the element on the helper
        /// </summary>
        /// <param name="helper">The UIHelper to attach the element to</param>
        public override void Create(UIHelperBase helper)
        {
            UISlider slider = helper.AddSlider(readableName, min, max, step, value, IgnoredFunction) as UISlider;
            slider.enabled = enabled;
            slider.name = uniqueName;
            slider.tooltip = value.ToString();
            slider.eventValueChanged += new PropertyChangedEventHandler<float>(delegate (UIComponent component, float newValue)
            {
                value = newValue;
                slider.tooltip = value.ToString();
                slider.RefreshTooltip();
            });
        }
    }

    /// <summary>
    /// Dropdown option
    /// </summary>
    public class OptionsDropdown : OptionsItemBase
    {
        /// <summary>
        /// The default value of the object, or the saved value if loaded.
        /// </summary>
        public string value
        {
            get { return (string)m_value; }
            set { m_value = value; }
        }
        /// <summary>
        /// All available options to select in the dropdown
        /// </summary>
        public string[] options = null;

        /// <summary>
        /// Create the element on the helper
        /// </summary>
        /// <param name="helper">The UIHelper to attach the element to</param>
        public override void Create(UIHelperBase helper)
        {
            UIDropDown dropdown = helper.AddDropdown(readableName, options, 0, IgnoredFunction) as UIDropDown;
            dropdown.enabled = enabled;
            dropdown.name = uniqueName;
            dropdown.tooltip = readableName;
            dropdown.eventSelectedIndexChanged += new PropertyChangedEventHandler<int>(delegate (UIComponent component, int newValue)
            {
                value = dropdown.selectedValue;
                dropdown.tooltip = readableName;
                dropdown.RefreshTooltip();
            });
        }
    }
}
