using ColossalFramework.UI;
using UnityEngine;

namespace Mapper.Panels
{
    class WhatsNewPanel : UIPanel
    {
        public override void Awake()
        {
            this.isInteractive = true;
            this.enabled = true;
            this.width = 300;
            this.height = 400;

            base.Awake();
        }

        public override void Start()
        {
            base.Start();

            UILabel labelTitle = this.AddUIComponent<UILabel>();
            labelTitle.text = "What's new?";
            labelTitle.textScale = 1.3f;
            labelTitle.autoSize = true;
            labelTitle.relativePosition = new Vector3(4, 4);

            CreateWhatsNewMessage();
            
            this.relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2), Mathf.Floor((GetUIView().fixedHeight - height) / 2));
            this.backgroundSprite = "UnlockingPanel2";
            this.atlas = CustomUI.UIUtils.GetAtlas("Ingame");
        }

        private void CreateWhatsNewMessage()
        {
            UILabel label = this.AddUIComponent<UILabel>();
            label.wordWrap = true;
            label.autoHeight = true;
            label.width = 260;
            label.padding = new RectOffset(0, 40, 0, 0);
            label.text =    "- Supports the <color#c8f582>Road Namer</color> mod, and exports all road names (thanks to tony56a).\n\n" +
                            "- Should work in <color#c8f582>JOSM</color> without throwing an error (thanks again to tony56a).\n\n" +
                            "- Some backend work.";
            label.textScale = 0.6f;
            label.relativePosition = new Vector2(40, 40);
            label.processMarkup = true;

            UIButton closeButton = CustomUI.UIUtils.CreateButton(this);
            closeButton.eventClicked += CloseButton_eventClicked;
            closeButton.textPadding = new RectOffset(6, 6, 6, 6);
            closeButton.size = new Vector2(220, 30);
            closeButton.relativePosition = label.relativePosition + new Vector3(0, label.height + 20);
            closeButton.text = "Close";

            this.height = closeButton.relativePosition.y + closeButton.height + 20;
        }

        private void CloseButton_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            this.Hide();
        }
    }
}
