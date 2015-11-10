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
            this.width = 300;

            mainHelper = new UIHelper(this);

            base.Awake();
        }

        public override void Start()
        {
            base.Start();

            UILabel labelTitle = this.AddUIComponent<UILabel>();
            labelTitle.text = "Cimtographer";
            labelTitle.textScale = 1.3f;

            CreateOptions();
                                   
            UIButton buttonGenerate = mainHelper.AddButton("Export", ButtonGenerate_eventClick) as UIButton;

            UISprite betaSprite = this.AddUIComponent<UISprite>();
            betaSprite.spriteName = "IconMessage";
            betaSprite.size = new Vector2(64, 64);

            this.relativePosition = new Vector3(20, 20);
            this.backgroundSprite = "MenuPanel2";
            this.autoLayout = true;
            this.autoLayoutDirection = LayoutDirection.Vertical;
            this.autoLayoutPadding = new RectOffset(5, 5, 5, 5);

            this.FitChildren();
            this.PerformLayout();

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
            UIPanel exportWhatGroupComponent = exportWhatGroupHelper.self as UIPanel;

            exportWhatGroupComponent.autoSize = true;
            exportWhatGroupComponent.FitTo(mainHelper.self as UIPanel);
            exportWhatGroupComponent.autoLayout = true;
            exportWhatGroupComponent.wrapLayout = true;
            exportWhatGroupComponent.autoLayoutDirection = LayoutDirection.Horizontal;

            /*UIPanel scrollContainer = exportWhatGroupComponent.AddUIComponent<UIPanel>();

            scrollContainer.autoSize = true;
            scrollContainer.FitTo(exportWhatGroupComponent);
            scrollContainer.autoLayout = true;
            scrollContainer.autoLayoutDirection = LayoutDirection.Horizontal;
            scrollContainer.clipChildren = true;

            UIScrollablePanel exportWhatScrollPanel = scrollContainer.AddUIComponent<UIScrollablePanel>();
            UIScrollbar exportWhatScrollBar = scrollContainer.AddUIComponent<UIScrollbar>();
            UISprite thumbSprite = exportWhatScrollBar.AddUIComponent<UISprite>();

            exportWhatScrollBar.thumbObject = thumbSprite;

            exportWhatScrollPanel.minimumSize = new Vector2(200, 150);
            exportWhatScrollPanel.maximumSize = new Vector2(10000, 150);
            exportWhatScrollPanel.autoLayout = true;
            exportWhatScrollPanel.autoLayoutDirection = LayoutDirection.Vertical;
            exportWhatScrollPanel.clipChildren = true;
            exportWhatScrollPanel.autoSize = true;
            exportWhatScrollPanel.backgroundSprite = "InfoPanelBack";
            exportWhatScrollPanel.verticalScrollbar = exportWhatScrollBar;
            exportWhatScrollPanel.eventMouseWheel += ExportWhatScrollPanel_eventMouseWheel;

            exportWhatScrollBar.height = exportWhatScrollPanel.height;
            exportWhatScrollBar.width = 20;*/

            CreateOptionList(exportWhatGroupComponent, MapperOptionsManager.Instance().exportOptions);

            /*scrollContainer.FitChildren();

            exportWhatScrollPanel.PerformLayout();
            scrollContainer.PerformLayout();*/
            exportWhatGroupComponent.FitChildren();
            exportWhatGroupComponent.PerformLayout();
        }

        private void ExportWhatScrollPanel_eventMouseWheel(UIComponent component, UIMouseEventParameter eventParam)
        {
            UIScrollablePanel componentScroll = component as UIScrollablePanel;
            componentScroll.scrollPosition += eventParam.moveDelta;
        }

        private void CreateOptionList(UIComponent parent, Dictionary<string, OptionItem> options)
        {
            UIHelper parentUIHelper = new UIHelper(parent);

            foreach (KeyValuePair<string, OptionItem> option in options)
            {
                UICheckBox checkboxOption = parentUIHelper.AddCheckbox(option.Value.readableLabel, option.Value.enabled, (bool isChecked) => {
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
            }
        }
    }
}
