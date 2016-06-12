using ColossalFramework.UI;
using UnityEngine;

namespace CimTools.v2.Utilities
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
        /// Creates a color field on the component
        /// </summary>
        /// <param name="parent">The component to add the color field to</param>
        /// <returns>A new color field</returns>
        public UIColorField CreateColorField(UIComponent parent)
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
        public void ResizeIcon(UISprite icon, Vector2 maxSize)
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
    }
}
