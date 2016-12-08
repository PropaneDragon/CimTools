using ColossalFramework.UI;
using UnityEngine;

namespace CimTools.v2.Elements
{
    public class UITitleBar : UIPanel
    {
        private UISprite m_icon;
        private UILabel m_title;
        private UIButton m_close;
        private UIDragHandle m_drag;
        private CimToolBase m_toolBase;

        public string iconSprite
        {
            get { return m_icon.spriteName; }
            set
            {
                if (m_icon != null)
                {
                    m_icon.spriteName = value;

                    if (m_icon.atlas == null && m_toolBase != null)
                    {
                        m_icon.atlas = m_toolBase.SpriteUtilities.GetAtlas("Ingame");
                    }

                    if (m_icon.spriteInfo != null && m_toolBase != null)
                    {
                        m_icon.size = m_icon.spriteInfo.pixelSize;
                        m_toolBase.UIUtilities.ResizeIcon(m_icon, new Vector2(32, 32));
                        m_icon.relativePosition = new Vector3(10, 5);
                    }
                }
            }
        }

        public UITextureAtlas iconAtlas
        {
            get { return m_icon.atlas; }
            set { m_icon.atlas = value; }
        }

        public UIButton closeButton
        {
            get { return m_close; }
        }

        public string title
        {
            get { return m_title.text; }
            set { m_title.text = value; }
        }

        public void Initialise(CimToolBase toolBase)
        {
            m_toolBase = toolBase;
        }

        public override void Awake()
        {
            base.Awake();

            m_icon = AddUIComponent<UISprite>();
            m_title = AddUIComponent<UILabel>();
            m_close = AddUIComponent<UIButton>();
            m_drag = AddUIComponent<UIDragHandle>();

            height = 40;
            width = 450;
            title = "(None)";
            iconSprite = "";
        }

        public override void Start()
        {
            if (m_toolBase != null)
            {
                base.Start();

                width = parent.width;
                relativePosition = Vector3.zero;
                isVisible = true;
                canFocus = true;
                isInteractive = true;

                m_drag.width = width - 50;
                m_drag.height = height;
                m_drag.relativePosition = Vector3.zero;
                m_drag.target = parent;

                m_icon.spriteName = iconSprite;
                m_icon.relativePosition = new Vector3(10, 5);

                m_title.relativePosition = new Vector3(50, 13);
                m_title.text = title;
                m_title.autoSize = false;
                m_title.textAlignment = UIHorizontalAlignment.Center;
                m_title.verticalAlignment = UIVerticalAlignment.Middle;

                m_close.atlas = m_toolBase.SpriteUtilities.GetAtlas("Ingame");
                m_close.relativePosition = new Vector3(width - 35, 2);
                m_close.normalBgSprite = "buttonclose";
                m_close.hoveredBgSprite = "buttonclosehover";
                m_close.pressedBgSprite = "buttonclosepressed";
                m_close.eventClick += (component, param) => parent.Hide();

                m_title.width = parent.width - m_title.relativePosition.x - m_close.width - 10;
            }
        }
    }
}
