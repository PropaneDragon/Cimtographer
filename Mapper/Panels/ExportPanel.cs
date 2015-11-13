using ColossalFramework.UI;
using UnityEngine;
using Mapper.Managers;
using System.Collections.Generic;
using Mapper.CustomUI;
using Mapper.OSM;

namespace Mapper.Panels
{
    class ExportPanel : UIPanel
    {
        int titleOffset = 40;
        Vector2 offset = Vector2.zero;

        public override void Awake()
        {
            this.isInteractive = true;
            this.enabled = true;
            this.width = 400;
            this.height = 500;

            base.Awake();
        }

        public override void Start()
        {
            base.Start();
            
            UILabel labelTitle = this.AddUIComponent<UILabel>();
            labelTitle.text = "Cimtographer";
            labelTitle.textScale = 1.3f;
            labelTitle.autoSize = true;
            labelTitle.relativePosition = new Vector3(4, 4);

            CreateLeftPanel();
            CreateRightPanel();
            CreateBottomMessage();

            this.relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2), Mathf.Floor((GetUIView().fixedHeight - height) / 2));
            this.backgroundSprite = "UnlockingPanel2";
            this.atlas = CustomUI.UIUtils.GetAtlas("Ingame");
        }

        private void CreateLeftPanel()
        {
            offset = new Vector2(4, titleOffset);

            CreateOptions();
        }

        private void CreateBottomMessage()
        {
            UISprite infoSprite = this.AddUIComponent<UISprite>();
            infoSprite.spriteName = "IconMessage";
            infoSprite.size = new Vector2(32, 32);
            infoSprite.relativePosition = new Vector3(4, height - 40);

            UILabel label = this.AddUIComponent<UILabel>();
            label.text = "Remember to post your maps to http://reddit.com/r/cimtographer,\nand show us what you've made!";
            label.textScale = 0.6f;
            label.size = new Vector2(600, 40);
            label.width = 600;
            label.relativePosition = new Vector2(40, height - 40);
        }

        private void CreateRightPanel()
        {
            offset = new Vector2(width / 2, titleOffset);

            CreateInfoMessage();

            UIButton buttonGenerate = CustomUI.UIUtils.CreateButton(this);
            buttonGenerate.eventClicked += ButtonGenerate_eventClicked;
            buttonGenerate.textPadding = new RectOffset(6, 6, 6, 6);
            buttonGenerate.autoSize = true;
            buttonGenerate.relativePosition = offset;
            buttonGenerate.text = "Generate OSM map";
        }

        private void CreateInfoMessage()
        {
            UISprite betaSprite = this.AddUIComponent<UISprite>();
            betaSprite.spriteName = "IconMessage";
            betaSprite.size = new Vector2(32, 32);
            betaSprite.relativePosition = offset;

            UILabel labelTitle = this.AddUIComponent<UILabel>();
            labelTitle.text = "Oooo, shiny! Cimtographer's \nhad a rewrite. Check out\nwhat's changed on the\nworkshop page.";
            labelTitle.textScale = 0.6f;
            labelTitle.size = new Vector2(180, 40);
            labelTitle.relativePosition = offset + new Vector2(42, 2);

            offset += new Vector2(0, 70);
        }

        private void CreateOptions()
        {
            CreateOptionList(MapperOptionsManager.Instance().exportOptions);
        }

        private void CreateOptionList(Dictionary<string, OptionItem> options)
        {
            foreach (KeyValuePair<string, OptionItem> option in options)
            {
                UICheckBox checkboxOption = CustomUI.UIUtils.CreateCheckBox(this);
                checkboxOption.eventCheckChanged += CheckboxOption_eventCheckChanged;
                checkboxOption.name = option.Key;
                checkboxOption.text = option.Value.readableLabel;
                checkboxOption.label.text = option.Value.readableLabel;
                checkboxOption.isChecked = option.Value.enabled;

                checkboxOption.width = 200;
                checkboxOption.height = 40;
                checkboxOption.relativePosition = offset;

                offset += new Vector2(0, 20);
            }
        }

        private void ButtonGenerate_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            OSMExportNew osmExporter = new OSMExportNew();
            osmExporter.Export();
        }

        private void CheckboxOption_eventCheckChanged(UIComponent component, bool value)
        {
            UICheckBox checkbox = component as UICheckBox;

            if (MapperOptionsManager.Instance().exportOptions.ContainsKey(checkbox.name))
            {
                MapperOptionsManager.Instance().exportOptions[checkbox.name].enabled = value;
                Debug.Log("Set \"" + checkbox.name + "\" to " + value.ToString());
            }
            else
            {
                Debug.LogError("Could not find option \"" + checkbox.name + "\".");
            }
        }
    }
}
