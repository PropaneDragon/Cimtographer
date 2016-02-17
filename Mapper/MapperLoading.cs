using ColossalFramework.UI;
using ICities;
using Mapper.CimTools;
using Mapper.Panels;
using System;
using System.IO;
using UnityEngine;

namespace Mapper
{
    public class MapperLoading : LoadingExtensionBase
    {
        private GameObject exportPanelGameObject;
        private GameObject whatsNewPanelGameObject;
        private UIButton m_tabButton = null;

        private ExportPanel exportPanel;
        private WhatsNewPanel whatsNewPanel;
        private LoadMode lastLoadMode_;

        public override void OnCreated(ILoading loading)
        {
            try //So we don't fuck up loading the city
            {
                LoadSprites();

                CimToolsHandler.CimToolBase.DetailedLogger.Log("Loading mod");
                CimToolsHandler.CimToolBase.Changelog.DownloadChangelog();
                CimToolsHandler.CimToolBase.XMLFileOptions.Load();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

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

            if (m_tabButton == null)
            {
                GameObject buttonGameObject = UITemplateManager.GetAsGameObject("MainToolbarButtonTemplate");
                GameObject pageGameObject = UITemplateManager.GetAsGameObject("ScrollablePanelTemplate");
                m_tabButton = tabStrip.AddTab("Road Namer", buttonGameObject, pageGameObject, new Type[] { }) as UIButton;

                UITextureAtlas atlas = CimToolsHandler.CimToolBase.SpriteUtilities.GetAtlas("CimtographerIcons");

                m_tabButton.eventClicked += uiButton_eventClick;
                m_tabButton.tooltip = "Cimtographer";
                m_tabButton.foregroundSpriteMode = UIForegroundSpriteMode.Fill;

                if (atlas != null)
                {
                    m_tabButton.atlas = atlas;
                    m_tabButton.normalFgSprite = "ToolbarFGIcon";
                    m_tabButton.focusedFgSprite = "ToolbarFGIcon";
                    m_tabButton.hoveredFgSprite = "ToolbarFGIcon";
                    m_tabButton.disabledFgSprite = "ToolbarFGIcon";
                    m_tabButton.pressedFgSprite = "ToolbarFGIcon";
                    m_tabButton.focusedBgSprite = "ToolbarBGFocused";
                    m_tabButton.hoveredBgSprite = "ToolbarBGHovered";
                    m_tabButton.pressedBgSprite = "ToolbarBGPressed";
                }
                else
                {
                    Debug.LogError("Cimtographer: Could not find atlas.");
                }
            }
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

        /// <summary>
        /// Loads all custom sprites
        /// </summary>
        private void LoadSprites()
        {
            bool atlasSuccess = CimToolsHandler.CimToolBase.SpriteUtilities.InitialiseAtlas(CimToolsHandler.CimToolBase.Path.GetModPath() + Path.DirectorySeparatorChar + "UIIcons.png", "CimtographerIcons");

            if (atlasSuccess)
            {
                bool spriteSuccess = true;

                spriteSuccess = CimToolsHandler.CimToolBase.SpriteUtilities.AddSpriteToAtlas(new Rect(new Vector2(2, 2), new Vector2(36, 36)), "ToolbarFGIcon", "CimtographerIcons") && spriteSuccess;
                spriteSuccess = CimToolsHandler.CimToolBase.SpriteUtilities.AddSpriteToAtlas(new Rect(new Vector2(40, 2), new Vector2(43, 49)), "ToolbarBGPressed", "CimtographerIcons") && spriteSuccess;
                spriteSuccess = CimToolsHandler.CimToolBase.SpriteUtilities.AddSpriteToAtlas(new Rect(new Vector2(85, 2), new Vector2(43, 49)), "ToolbarBGHovered", "CimtographerIcons") && spriteSuccess;
                spriteSuccess = CimToolsHandler.CimToolBase.SpriteUtilities.AddSpriteToAtlas(new Rect(new Vector2(130, 2), new Vector2(43, 49)), "ToolbarBGFocused", "CimtographerIcons") && spriteSuccess;

                if (!spriteSuccess)
                {
                    Debug.LogError("Cimtographer: Failed to load some sprites!");
                }
            }
            else
            {
                Debug.LogError("Cimtographer: Failed to load the atlas!");
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
        }
    }
}

