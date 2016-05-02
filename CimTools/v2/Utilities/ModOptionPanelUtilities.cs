using CimTools.v2.Data;
using CimTools.v2.Logging;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CimTools.v2.Utilities
{
    /// <summary>
    /// When the user has saved the options
    /// </summary>
    public delegate void OptionPanelSavedEventHandler();

    /// <summary>
    /// Handles options on the mod option panel ingame
    /// </summary>
    public class ModOptionPanelUtilities
    {
        private CimToolBase _toolBase = null;
        private Dictionary<string, List<OptionsItemBase>> _options = new Dictionary<string, List<OptionsItemBase>>();
        private OptionPanelSavedData _savedData;

        public OptionPanelSavedData Data
        {
            get { return _savedData; }
        }

        /// <summary>
        /// When the options have been saved by the user.
        /// </summary>
        public event OptionPanelSavedEventHandler OnOptionPanelSaved;

        /// <summary>
        /// Handles options on the mod option panel ingame
        /// </summary>
        /// <param name="toolBase">The tool base</param>
        public ModOptionPanelUtilities(CimToolBase toolBase)
        {
            _toolBase = toolBase;
            _toolBase.DetailedLogger.Log("Init ModOptionPanelUtilities");

            _savedData = new OptionPanelSavedData();
            toolBase.XMLFileOptions.AddObjectToSave(_savedData);
        }

        /// <summary>
        /// Creates options on a panel using the helper
        /// </summary>
        /// <param name="helper">The UIHelper to put the options on</param>
        /// <param name="options">The options to put inside the group</param>
        /// <param name="groupName">The title of the group in the options panel.</param>
        /// <param name="translationId">The ID of the translation to apply to the name.</param>
        public void CreateOptions(UIHelperBase helper, List<OptionsItemBase> options, string groupName = "Options", string translationId = null)
        {
            _options[groupName] = options;
            LoadOptions();

            UIHelperBase optionGroup = helper.AddGroup(groupName);

            foreach (OptionsItemBase option in options)
            {
                option.Create(optionGroup);
                option.Translate(_toolBase.Translation);

                _toolBase.Translation.OnLanguageChanged += delegate (string languageIdentifier)
                {
                    _toolBase.DetailedLogger.Log("Translating option " + option.translationIdentifier);
                    option.Translate(_toolBase.Translation);
                };
            }

            UIButton saveButton = optionGroup.AddButton("Apply", SaveOptions) as UIButton;
            saveButton.width = 120;
            saveButton.color = new Color32(0, 255, 0, 255);

            TranslateGroupItems(saveButton, optionGroup, translationId);

            _toolBase.Translation.OnLanguageChanged += delegate (string languageIdentifier)
            {
                _toolBase.DetailedLogger.Log("Translating option group " + groupName);
                TranslateGroupItems(saveButton, optionGroup, translationId);
            };
        }

        private void TranslateGroupItems(UIButton saveButton, UIHelperBase group, string groupTranslationId)
        {
            if (saveButton != null && group != null)
            {
                if (_toolBase.Translation.HasTranslation("OptionButton_Apply"))
                {
                    saveButton.text = _toolBase.Translation.GetTranslation("OptionButton_Apply");
                }
                else
                {
                    _toolBase.DetailedLogger.LogWarning("There is no option group translation for the options apply button!");
                }

                if (groupTranslationId != null && _toolBase.Translation.HasTranslation("OptionGroup_" + groupTranslationId))
                {
                    _toolBase.DetailedLogger.Log("Translating option " + groupTranslationId);

                    UIHelper mainHelper = group as UIHelper;

                    if (mainHelper != null)
                    {
                        _toolBase.DetailedLogger.Log("Found group helper");

                        UIComponent uiComponent = mainHelper.self as UIComponent;

                        if (uiComponent != null)
                        {
                            _toolBase.DetailedLogger.Log("Found group UIComponent");

                            UIComponent parent = uiComponent.parent;
                            UILabel label = parent.Find<UILabel>("Label");

                            if (label != null)
                            {
                                _toolBase.DetailedLogger.Log("Found group label");
                                label.text = _toolBase.Translation.GetTranslation("OptionGroup_" + groupTranslationId);
                            }
                            else
                            {
                                _toolBase.DetailedLogger.LogWarning("The group has no label to translate!");
                            }
                        }
                        else
                        {
                            _toolBase.DetailedLogger.LogWarning("Could not find the UIComponent for the group!");
                        }
                    }
                    else
                    {
                        _toolBase.DetailedLogger.LogWarning("There is no helper for the group!");
                    }
                }
                else
                {
                    _toolBase.DetailedLogger.LogWarning("There is no option group translation for " + groupTranslationId);
                }
            }
            else
            {
                _toolBase.DetailedLogger.LogWarning("The button or the helper are invalid");
            }
        }

        /// <summary>
        /// Manually save the UI options to the persistent options. When this is called
        /// by the class internally, you can get the event using OnOptionPanelSaved()
        /// </summary>
        public void SaveOptions()
        {
            foreach (KeyValuePair<string, List<OptionsItemBase>> optionGroup in _options)
            {
                foreach (OptionsItemBase option in optionGroup.Value)
                {
                    if (option.uniqueName != "")
                    {
                        if (_savedData.data.ContainsKey(option.uniqueName))
                        {
                            _savedData.data[option.uniqueName] = option.m_value;
                        }
                        else
                        {
                            _savedData.data.Add(option.uniqueName, option.m_value);
                        }
                    }
                    else
                    {
                        _toolBase.DetailedLogger.LogWarning("An option had no unique name, so wasn't saved! (" + option.GetType().ToString() + ")");
                    }
                }
            }

            _toolBase.XMLFileOptions.Save();

            if (OnOptionPanelSaved != null)
            {
                OnOptionPanelSaved();
            }
        }

        /// <summary>
        /// Manually load the UI options onto the existing panel elements.
        /// </summary>
        public void LoadOptions()
        {
            _toolBase.DetailedLogger.Log("Loading options up");
            _toolBase.XMLFileOptions.Load();

            foreach (KeyValuePair<string, List<OptionsItemBase>> optionGroup in _options)
            {
                _toolBase.DetailedLogger.Log("Loading option group " + optionGroup.Key);
                foreach (OptionsItemBase option in optionGroup.Value)
                {
                    _toolBase.DetailedLogger.Log("Loading option " + option.uniqueName);

                    if (_savedData.data.ContainsKey(option.uniqueName))
                    {
                        object value = _savedData.data[option.uniqueName];
                        option.m_value = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the value of the option as type T
        /// </summary>
        /// <typeparam name="T">The type the option should be returned as</typeparam>
        /// <param name="uniqueName">The option name</param>
        /// <param name="value">The value to insert the returned data into</param>
        /// <returns></returns>
        public bool GetOptionValue<T>(string uniqueName, ref T value)
        {
            bool found = false;

            foreach (KeyValuePair<string, List<OptionsItemBase>> optionGroup in _options)
            {
                foreach (OptionsItemBase option in optionGroup.Value)
                {
                    if (option.uniqueName == uniqueName)
                    {
                        value = (T)Convert.ChangeType(option.m_value, typeof(T));
                        found = true;

                        break;
                    }
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
        /// <summary>
        /// The object value of this item. Use value to get and set the value as the correct type.
        /// </summary>
        public object m_value = default(object);

        /// <summary>
        /// The unique option name. Can't clash with any other option names
        /// or you'll lose data.
        /// </summary>
        public string uniqueName = "";

        /// <summary>
        /// The name that appears on the UI.
        /// </summary>
        //public string readableName = "";

        /// <summary>
        /// The identifier to link to the translatable attribute.
        /// </summary>
        public string translationIdentifier = "";

        /// <summary>
        /// Whether the option is enabled or not
        /// </summary>
        public bool enabled = true;

        /// <summary>
        /// The UIComponent of this item
        /// </summary>
        protected UIComponent component = null;

        /// <summary>
        /// Create the element on the helper
        /// </summary>
        /// <param name="helper">The UIHelper to attach the element to</param>
        public abstract UIComponent Create(UIHelperBase helper);

        /// <summary>
        /// Called when translations change
        /// </summary>
        /// <param name="translation">The translation data</param>
        public abstract void Translate(Translation translation);

        /// <summary>
        /// Required for creating UI elements. Ignored by this utility.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ignored"></param>
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
            get
            {
                bool? convertedValue = Convert.ChangeType(m_value, typeof(bool)) as bool?;
                return convertedValue.HasValue ? convertedValue.Value : false;
            }
            set { m_value = value; }
        }

        /// <summary>
        /// Create the element on the helper
        /// </summary>
        /// <param name="helper">The UIHelper to attach the element to</param>
        public override UIComponent Create(UIHelperBase helper)
        {
            UICheckBox checkBox = helper.AddCheckbox("translate me", value, IgnoredFunction) as UICheckBox;
            checkBox.readOnly = !enabled;
            checkBox.name = uniqueName;
            checkBox.disabledColor = new Color32(100, 100, 100, 255);
            checkBox.eventCheckChanged += new PropertyChangedEventHandler<bool>(delegate (UIComponent component, bool newValue)
            {
                value = newValue;
            });

            component = checkBox;
            return checkBox;
        }

        public override void Translate(Translation translation)
        {
            UICheckBox uiObject = component as UICheckBox;
            uiObject.tooltip = translation.GetTranslation("OptionTooltip_" + (translationIdentifier == "" ? uniqueName : translationIdentifier));
            uiObject.label.text = translation.GetTranslation("Option_" + (translationIdentifier == "" ? uniqueName : translationIdentifier));

            try
            {
                uiObject.RefreshTooltip();
            }
            catch
            {
                //Tooltip isn't created yet, so can't be refreshed. Not nice :(
            }
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
            get
            {
                float? convertedValue = Convert.ChangeType(m_value, typeof(float)) as float?;
                return convertedValue.HasValue ? convertedValue.Value : 0f;
            }
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
        public override UIComponent Create(UIHelperBase helper)
        {
            UISlider slider = helper.AddSlider("translate me", min, max, step, value, IgnoredFunction) as UISlider;
            slider.enabled = enabled;
            slider.name = uniqueName;
            slider.tooltip = value.ToString();
            slider.eventValueChanged += new PropertyChangedEventHandler<float>(delegate (UIComponent component, float newValue)
            {
                value = newValue;
                slider.tooltip = value.ToString();
                slider.RefreshTooltip();
            });

            component = slider;
            return component;
        }

        public override void Translate(Translation translation)
        {
            UISlider uiObject = component as UISlider;

            UIPanel sliderParent = uiObject.parent as UIPanel;
            if (sliderParent != null)
            {
                UILabel label = sliderParent.Find<UILabel>("Label");

                if (label != null)
                {
                    label.text = translation.GetTranslation("Option_" + (translationIdentifier == "" ? uniqueName : translationIdentifier));
                }
            }
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
        public override UIComponent Create(UIHelperBase helper)
        {
            int selectedIndex = 0;
            for (; selectedIndex < options.Length; ++selectedIndex)
            {
                if (options[selectedIndex] == value)
                {
                    break;
                }
            }

            UIDropDown dropdown = helper.AddDropdown("translate me", options, selectedIndex, IgnoredFunction) as UIDropDown;
            dropdown.enabled = enabled;
            dropdown.name = uniqueName;
            dropdown.tooltip = value;
            dropdown.eventSelectedIndexChanged += new PropertyChangedEventHandler<int>(delegate (UIComponent component, int newValue)
            {
                value = dropdown.selectedValue;
            });

            component = dropdown;
            return dropdown;
        }

        public override void Translate(Translation translation)
        {
            UIDropDown uiObject = component as UIDropDown;
            uiObject.tooltip = translation.GetTranslation("OptionTooltip_" + (translationIdentifier == "" ? uniqueName : translationIdentifier));

            string[] translatedItems = translation.GetTranslation("Option_" + (translationIdentifier == "" ? uniqueName : translationIdentifier)).Split('|');
            string[] uiItems = uiObject.items;

            if (translatedItems.Length == uiItems.Length + 1)
            {
                for (int index = 0; index < translatedItems.Length; ++index)
                {
                    uiItems[index] = translatedItems[index + 1];
                }
            }

            if (translatedItems.Length == uiItems.Length + 1 || translatedItems.Length == 1)
            {
                UIPanel sliderParent = uiObject.parent as UIPanel;
                if (sliderParent != null)
                {
                    UILabel label = sliderParent.Find<UILabel>("Label");

                    if (label != null)
                    {
                        label.text = translatedItems[0];
                    }
                }
            }

            try
            {
                uiObject.RefreshTooltip();
            }
            catch
            {
                //Tooltip isn't created yet, so can't be refreshed. Not nice :(
            }
        }
    }

    /// <summary>
    /// Dropdown option
    /// </summary>
    public class OptionsSpace : OptionsItemBase
    {
        /// <summary>
        /// The default value of the object, or the saved value if loaded.
        /// </summary>
        public object value
        {
            get { return null; }
            set { m_value = value; }
        }

        /// <summary>
        /// Set the spacing of the UI between this spacer.
        /// </summary>
        public int spacing = 15;

        /// <summary>
        /// Create the element on the helper
        /// </summary>
        /// <param name="helper">The UIHelper to attach the element to</param>
        public override UIComponent Create(UIHelperBase helper)
        {
            return helper.AddSpace(spacing) as UIComponent;
        }

        public override void Translate(Translation translation)
        {
        }
    }
}
