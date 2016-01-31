using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace CimTools.V1.Utilities
{
    /// <summary>
    /// Utilities for adding items to the UI
    /// </summary>
    public class UIUtilities
    {
        // Figuring all this was a pain (no documentation whatsoever)
        // So if your are using it for your mod consider thanking me (SamsamTS)
        // Extended Public Transport UI's code helped me a lot so thanks a lot AcidFire

        /// <summary>
        /// Creates a button on the component
        /// </summary>
        /// <param name="parent">The component to add the button to</param>
        /// <returns>A new button</returns>
        public static UIButton CreateButton(UIComponent parent)
        {
            UIButton button = (UIButton)parent.AddUIComponent<UIButton>();
            button.atlas = defaultAtlas;
            button.size = new Vector2(90f, 30f);
            button.textScale = 0.9f;
            button.normalBgSprite = "ButtonMenu";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.canFocus = false;

            return button;
        }

        /// <summary>
        /// Creates a checkbox on the component
        /// </summary>
        /// <param name="parent">The component to add the checbox to</param>
        /// <returns>A new checkbox</returns>
        public static UICheckBox CreateCheckBox(UIComponent parent)
        {
            UICheckBox checkBox = (UICheckBox)parent.AddUIComponent<UICheckBox>();

            checkBox.width = 300f;
            checkBox.height = 20f;
            checkBox.clipChildren = true;

            UISprite sprite = checkBox.AddUIComponent<UISprite>();
            sprite.spriteName = "ToggleBase";
            sprite.size = new Vector2(16f, 16f);
            sprite.relativePosition = Vector3.zero;

            checkBox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
            ((UISprite)checkBox.checkedBoxObject).atlas = defaultAtlas;
            ((UISprite)checkBox.checkedBoxObject).spriteName = "ToggleBaseFocused";
            checkBox.checkedBoxObject.size = new Vector2(16f, 16f);
            checkBox.checkedBoxObject.relativePosition = Vector3.zero;

            checkBox.label = checkBox.AddUIComponent<UILabel>();
            checkBox.label.atlas = defaultAtlas;
            checkBox.label.text = " ";
            checkBox.label.textScale = 0.9f;
            checkBox.label.relativePosition = new Vector3(22f, 2f);

            return checkBox;
        }

        /// <summary>
        /// Creates a text field on the component
        /// </summary>
        /// <param name="parent">The component to add the text field to</param>
        /// <returns>A new text field</returns>
        public static UITextField CreateTextField(UIComponent parent)
        {
            UITextField textField = parent.AddUIComponent<UITextField>();
            textField.atlas = defaultAtlas;
            textField.size = new Vector2(90f, 20f);
            textField.padding = new RectOffset(6, 6, 3, 3);
            textField.builtinKeyNavigation = true;
            textField.isInteractive = true;
            textField.readOnly = false;
            textField.horizontalAlignment = UIHorizontalAlignment.Center;
            textField.selectionSprite = "EmptySprite";
            textField.selectionBackgroundColor = new Color32(0, 172, 234, 255);
            textField.normalBgSprite = "TextFieldPanelHovered";
            textField.textColor = new Color32(0, 0, 0, 255);
            textField.disabledTextColor = new Color32(0, 0, 0, 128);
            textField.color = new Color32(255, 255, 255, 255);

            return textField;
        }

        /// <summary>
        /// Creates a dropdown on the component
        /// </summary>
        /// <param name="parent">The component to add the dropdown to</param>
        /// <returns>A new dropdown</returns>
        public static UIDropDown CreateDropDown(UIComponent parent)
        {
            UIDropDown dropDown = parent.AddUIComponent<UIDropDown>();
            dropDown.atlas = defaultAtlas;
            dropDown.size = new Vector2(90f, 30f);
            dropDown.listBackground = "GenericPanelLight";
            dropDown.itemHeight = 30;
            dropDown.itemHover = "ListItemHover";
            dropDown.itemHighlight = "ListItemHighlight";
            dropDown.normalBgSprite = "ButtonMenu";
            dropDown.disabledBgSprite = "ButtonMenuDisabled";
            dropDown.hoveredBgSprite = "ButtonMenuHovered";
            dropDown.focusedBgSprite = "ButtonMenu";
            dropDown.listWidth = 90;
            dropDown.listHeight = 500;
            dropDown.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            dropDown.popupColor = new Color32(45, 52, 61, 255);
            dropDown.popupTextColor = new Color32(170, 170, 170, 255);
            dropDown.zOrder = 1;
            dropDown.textScale = 0.8f;
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            dropDown.selectedIndex = 0;
            dropDown.textFieldPadding = new RectOffset(8, 0, 8, 0);
            dropDown.itemPadding = new RectOffset(14, 0, 8, 0);

            UIButton button = dropDown.AddUIComponent<UIButton>();
            dropDown.triggerButton = button;
            button.atlas = defaultAtlas;
            button.text = "";
            button.size = dropDown.size;
            button.relativePosition = new Vector3(0f, 0f);
            button.textVerticalAlignment = UIVerticalAlignment.Middle;
            button.textHorizontalAlignment = UIHorizontalAlignment.Left;
            button.normalFgSprite = "IconDownArrow";
            button.hoveredFgSprite = "IconDownArrowHovered";
            button.pressedFgSprite = "IconDownArrowPressed";
            button.focusedFgSprite = "IconDownArrowFocused";
            button.disabledFgSprite = "IconDownArrowDisabled";
            button.foregroundSpriteMode = UIForegroundSpriteMode.Fill;
            button.horizontalAlignment = UIHorizontalAlignment.Right;
            button.verticalAlignment = UIVerticalAlignment.Middle;
            button.zOrder = 0;
            button.textScale = 0.8f;

            dropDown.eventSizeChanged += new PropertyChangedEventHandler<Vector2>((c, t) =>
            {
                button.size = t; dropDown.listWidth = (int)t.x;
            });

            return dropDown;
        }

        /// <summary>
        /// Creates a color field on the component
        /// </summary>
        /// <param name="parent">The component to add the color field to</param>
        /// <returns>A new color field</returns>
        public static UIColorField CreateColorField(UIComponent parent)
        {
            //UIColorField colorField = parent.AddUIComponent<UIColorField>();
            // Creating a ColorField from scratch is tricky. Cloning an existing one instead.
            // Probably doesn't work when on main menu screen and such as no ColorField exists.
            UIColorField colorField = Object.Instantiate(Object.FindObjectOfType<UIColorField>().gameObject).GetComponent<UIColorField>();
            parent.AttachUIComponent(colorField.gameObject);

            colorField.size = new Vector2(40f, 26f);
            colorField.normalBgSprite = "ColorPickerOutline";
            colorField.hoveredBgSprite = "ColorPickerOutlineHovered";
            colorField.selectedColor = Color.white;
            colorField.pickerPosition = UIColorField.ColorPickerPosition.RightAbove;

            return colorField;
        }

        /// <summary>
        /// Resizes a sprite to fit within certain bounds. It doesn't stretch the sprite
        /// and keeps the correct ratio.
        /// </summary>
        /// <param name="icon">The sprite to resize</param>
        /// <param name="maxSize">The maximum size of the sprite</param>
        public static void ResizeIcon(UISprite icon, Vector2 maxSize)
        {
            if (icon.height == 0) return;

            float ratio = icon.width / icon.height;

            if (icon.width > maxSize.x)
            {
                icon.width = maxSize.x;
                icon.height = maxSize.x / ratio;
            }

            if (icon.height > maxSize.y)
            {
                icon.height = maxSize.y;
                icon.width = maxSize.y * ratio;
            }
        }

        /// <summary>
        /// The default game atlas
        /// </summary>
        public static UITextureAtlas defaultAtlas
        {
            get { return GetAtlas("Ingame"); }
        }

        /// <summary>
        /// Gets an atlas by name
        /// </summary>
        /// <param name="name">The atlas name to retrieve</param>
        /// <returns>The named atlas, or null if none is found.</returns>
        public static UITextureAtlas GetAtlas(string name)
        {
            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++)
            {
                if (atlases[i].name == name)
                    return atlases[i];
            }

            return null;
        }
    }
}
