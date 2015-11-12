using ColossalFramework.UI;
using UnityEngine;
using Mapper.Managers;
using System.Collections.Generic;
using Mapper.CustomUI;
using System;
using ICities;
using Mapper.OSM;

namespace Mapper.Panels
{
    class ExportPanel : UIPanel
    {
        private UIHelper mainHelper;

        public override void Awake()
        {
            this.isInteractive = true;
            this.enabled = true;
            this.width = 400;
            this.height = 500;

            mainHelper = new UIHelper(this);

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

            CreateOptions();
            CreateRightPanel();

            this.relativePosition = new Vector3(20, 20);
            this.backgroundSprite = "MenuPanel2";

            //this.PerformLayout();

            this.backgroundSprite = "MenuPanel2";
        }

        private void ButtonGenerate_eventClick()
        {
            OSMExportNew osmExporter = new OSMExportNew();
            osmExporter.Export();
        }

        private void CreateOptions()
        {
            UIHelper exportWhatGroupHelper = mainHelper.AddGroup("Choose items to export") as UIHelper;
            UIPanel exportWhatGroupPanel = exportWhatGroupHelper.self as UIPanel;

            exportWhatGroupPanel.relativePosition = new Vector3(200, 800);
            exportWhatGroupPanel.autoLayout = true;
            exportWhatGroupPanel.wrapLayout = false;
            exportWhatGroupPanel.autoLayoutDirection = LayoutDirection.Vertical;
            exportWhatGroupPanel.padding = new RectOffset(5, 5, 2, 2);
            exportWhatGroupPanel.verticalSpacing = 2;
            exportWhatGroupPanel.maximumSize = new Vector2(100, 400);
            exportWhatGroupPanel.size = exportWhatGroupPanel.maximumSize;

            CreateOptionList(exportWhatGroupPanel, MapperOptionsManager.Instance().exportOptions);
        }

        private void CreateRightPanel()
        {
            UIHelper rightPanelGroupHelper = mainHelper.AddGroup("blah") as UIHelper;
            UIPanel rightPanel = rightPanelGroupHelper.self as UIPanel;

            rightPanel.relativePosition = new Vector3(400, 800);
            rightPanel.autoLayout = true;
            rightPanel.wrapLayout = false;
            rightPanel.autoLayoutDirection = LayoutDirection.Vertical;
            rightPanel.maximumSize = new Vector2(100, 400);
            rightPanel.size = rightPanel.maximumSize;

            CreateInfoMessage(rightPanel);

            UIButton buttonGenerate = rightPanelGroupHelper.AddButton("Export OSM map", ButtonGenerate_eventClick) as UIButton;
        }

        private void CreateInfoMessage(UIPanel parent)
        {
            UIPanel messagePanel = parent.AddUIComponent<UIPanel>();
            messagePanel.autoSize = true;
            messagePanel.autoLayout = true;
            messagePanel.autoLayoutDirection = LayoutDirection.Horizontal;

            UISprite betaSprite = messagePanel.AddUIComponent<UISprite>();
            betaSprite.spriteName = "IconMessage";
            betaSprite.size = new Vector2(64, 64);

            UILabel labelTitle = this.AddUIComponent<UILabel>();
            labelTitle.text = "Whoa! What's this?!";
            labelTitle.textScale = 1f;
            labelTitle.autoSize = true;

            //messagePanel.FitChildren();
        }

        private void CreateOptionList(UIComponent parent, Dictionary<string, OptionItem> options)
        {
            UIHelper parentUIHelper = new UIHelper(parent);

            foreach (KeyValuePair<string, OptionItem> option in options)
            {
                UICheckBox checkboxOption = parentUIHelper.AddCheckbox(option.Value.readableLabel, option.Value.enabled, (bool isChecked) => 
                {
                    if (MapperOptionsManager.Instance().exportOptions.ContainsKey(option.Key))
                    {
                        MapperOptionsManager.Instance().exportOptions[option.Key].enabled = isChecked;
                        Debug.Log("Set \"" + option.Key + "\" to " + option.Value.enabled.ToString());
                    }
                    else
                    {
                        Debug.LogError("Could not find option \"" + option.Key + "\".");
                    }
                }) as UICheckBox;

                checkboxOption.autoSize = true;
            }
        }
    }
}
