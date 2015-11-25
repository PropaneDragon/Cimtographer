using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using Mapper.Panels;
using Mapper.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mapper
{
    public class MapperLoading : LoadingExtensionBase
    {
        private GameObject exportPanelGameObject;
        private GameObject whatsNewPanelGameObject;
        private GameObject buttonObject;
        private GameObject buttonObject2;
        private UIButton menuButton;

        private ExportPanel exportPanel;
        private WhatsNewPanel whatsNewPanel;
        private LoadMode lastLoadMode_;

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame && mode != LoadMode.NewMap && mode != LoadMode.LoadMap)
                return;

            lastLoadMode_ = mode;

            UIView view = UIView.GetAView();
            UITabstrip tabStrip = null;

            whatsNewPanelGameObject = new GameObject("whatsNewPanel");
            this.whatsNewPanel = whatsNewPanelGameObject.AddComponent<WhatsNewPanel>();
            this.whatsNewPanel.transform.parent = view.transform;
            this.whatsNewPanel.Hide();

            exportPanelGameObject = new GameObject("exportPanel");
            this.exportPanel = exportPanelGameObject.AddComponent<ExportPanel>();
            this.exportPanel.transform.parent = view.transform;
            this.exportPanel.whatsNewPanel = whatsNewPanel;
            this.exportPanel.Hide();

            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame)
            {
                tabStrip =  ToolsModifierControl.mainToolbar.component as UITabstrip;
            }
            else
            {
                tabStrip = UIView.Find<UITabstrip>("MainToolstrip");
            }

            buttonObject = UITemplateManager.GetAsGameObject("MainToolbarButtonTemplate");
            buttonObject2 = UITemplateManager.GetAsGameObject("ScrollablePanelTemplate");
            menuButton = tabStrip.AddTab("cimtographerMod", buttonObject, buttonObject2, new Type[] { }) as UIButton;
            menuButton.eventClick += uiButton_eventClick;
        }

        private void uiButton_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {

            if (!this.exportPanel.isVisible)
            {
                this.exportPanel.isVisible = true;
                this.exportPanel.BringToFront();
                this.exportPanel.Show();

                if(this.exportPanel.scrollOptionsList != null)
                {
                    this.exportPanel.scrollOptionsList.DisplayAt(0);
                    this.exportPanel.scrollOptionsList.selectedIndex = 0;
                }
            }
            else
            {
                this.exportPanel.isVisible = false;
                this.exportPanel.Hide();
            }            
        }

        public override void OnLevelUnloading()
        {
            if (lastLoadMode_ != LoadMode.LoadGame && lastLoadMode_ != LoadMode.NewGame && lastLoadMode_ != LoadMode.NewMap && lastLoadMode_ != LoadMode.LoadMap)
                return;


            if (exportPanelGameObject != null)
            {
                GameObject.Destroy(exportPanelGameObject);
            }

            if(whatsNewPanelGameObject != null)
            {
                GameObject.Destroy(whatsNewPanelGameObject);
            }

            if (buttonObject != null)
            {
                GameObject.Destroy(buttonObject);
                GameObject.Destroy(buttonObject2);
                UIComponent.Destroy(menuButton);
            }
        }

       
    }
}

