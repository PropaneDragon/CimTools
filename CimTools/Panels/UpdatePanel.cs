using CimTools.Elements;
using CimTools.Workshop;
using ColossalFramework.UI;
using UnityEngine;

namespace CimTools.Panels
{
    /// <summary>
    /// A speech bubble which contains the latest changes to your mod.
    /// The bubble can be clicked and expanded to reveal new changes, rather than
    /// taking up loads of space.
    /// </summary>
    public class UpdatePanel : UIPanel
    {
        protected RectOffset m_UIPadding = new RectOffset(5, 5, 5, 5);
        private UITitleBar m_panelTitle;
        private UILabel m_infoLabel;

        /// <summary>
        /// Set and get a Changelog downloader. By default this uses the default Changelog
        /// instance, but you can pass your own through here if you wish to use your own.
        /// <para>
        /// Please note that if you are using the default Changelog instance you will need to set
        /// it up for your mod, otherwise you'll get no changes.
        /// </para>
        /// </summary>
        public Changelog m_changelogDownloader = Changelog.Instance();

        /// <summary>
        /// The initial message title when an update has been detected.
        /// </summary>
        public string m_updatedTitleMessage = "I've updated!";

        /// <summary>
        /// The initial message content when an update has been detected.
        /// </summary>
        public string m_updatedContentMessage = "<color#c8f582>Click here</color> to see what's changed";

        public override void Awake()
        {
            this.isInteractive = true;
            this.enabled = true;
            this.width = 200;
            this.height = 100;

            base.Awake();
        }

        public override void Start()
        {
            base.Start();

            m_panelTitle = this.AddUIComponent<UITitleBar>();
            m_panelTitle.title = m_updatedTitleMessage;

            m_infoLabel = this.AddUIComponent<UILabel>();
            m_infoLabel.width = 200 - m_UIPadding.left - m_UIPadding.right;
            m_infoLabel.wordWrap = true;
            m_infoLabel.processMarkup = true;
            m_infoLabel.autoHeight = true;
            m_infoLabel.text = m_updatedContentMessage;
            m_infoLabel.textScale = 0.6f;
            m_infoLabel.relativePosition = new Vector3(m_UIPadding.left, m_panelTitle.height + m_UIPadding.bottom);
            m_infoLabel.eventClicked += M_infoLabel_eventClicked;

            this.atlas = Utilities.UIUtilities.GetAtlas("Ingame");
            this.backgroundSprite = "InfoBubble";
            this.height = m_infoLabel.relativePosition.y + m_infoLabel.height + m_UIPadding.bottom + 20;
        }

        private void M_infoLabel_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            ShowUpdateInfo();
        }

        /// <summary>
        /// Sets the position of the panel based on the "speaky point" of the
        /// speech bubble it creates
        /// </summary>
        /// <param name="position">Position to set the panel to</param>
        public void setPositionSpeakyPoint(Vector2 position)
        {
            this.relativePosition = new Vector3(position.x, position.y - this.height);
        }

        private void ShowUpdateInfo()
        {
            float lastHeight = m_infoLabel.height;

            m_infoLabel.text = "Unable to retrieve the latest changes! Check on the workshop for the most recent changes.";

            if (!Changelog.Instance().DownloadInProgress && !Changelog.Instance().DownloadError)
            {
                m_infoLabel.text = Changelog.Instance().ChangesString;
            }
            else
            {
                Debug.LogError("Failed to download workshop changes!");
            }

            float heightDifference = m_infoLabel.height - lastHeight;

            this.height = m_infoLabel.relativePosition.y + m_infoLabel.height + m_UIPadding.bottom + 20;
            this.relativePosition -= new Vector3(0, heightDifference);
        }
    }
}
