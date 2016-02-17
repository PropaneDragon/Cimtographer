using ColossalFramework.UI;
using UnityEngine;
using Mapper.Managers;
using System.Collections.Generic;
using Mapper.CustomUI;
using Mapper.OSM;
using ColossalFramework.Steamworks;
using System;
using ColossalFramework;
using System.IO;
using System.Diagnostics;
using Mapper.Decompression;
using CimTools.V1.Panels;
using Mapper.CimTools;

namespace Mapper.Panels
{
    class ExportPanel : UIPanel
    {
        private int titleOffset = 40;
        private Vector2 offset = Vector2.zero;

        private UpdatePanel _updatePanel = null;
        public WhatsNewPanel whatsNewPanel = null;
        public UIFastList scrollOptionsList = null;
        private UIButton buttonGenerate = null;

        public override void Awake()
        {
            this.isInteractive = true;
            this.enabled = true;
            this.width = 400;
            this.height = 400;

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

            CreateTopMessage();
            CreateLeftPanel();
            CreateRightPanel();
            CreateBottomMessage();
            InitialiseUpdatePanel(this);

            this.relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2), Mathf.Floor((GetUIView().fixedHeight - height) / 2));
            this.backgroundSprite = "UnlockingPanel2";
            this.atlas = CustomUI.UIUtils.GetAtlas("Ingame");

        }

        private void InitialiseUpdatePanel(UIComponent parent)
        {
            _updatePanel = parent.AddUIComponent<UpdatePanel>();
            _updatePanel.SetPositionSpeakyPoint(new Vector2(parent.position.x, parent.position.y) + new Vector2(parent.size.x - 10, 10));
            _updatePanel.Initialise(CimToolsHandler.CimToolBase);
        }

        private void CreateTopMessage()
        {
            /*UISprite infoSprite = this.AddUIComponent<UISprite>();
            infoSprite.spriteName = "NotificationIconExtremelyHappy";
            infoSprite.size = new Vector2(32, 32);
            infoSprite.relativePosition = new Vector3(4, 40);*/

            UILabel label = this.AddUIComponent<UILabel>();
            label.text =    "";
            label.textScale = 0.6f;
            label.size = new Vector2(600, 40);
            label.width = 600;
            label.relativePosition = new Vector2(40, 40);
            label.textColor = new Color32(200, 245, 130, 255);
            label.processMarkup = true;
            label.eventClicked += TopLabel_eventClicked;

            titleOffset += 0;
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
            label.text =    "Remember to post your maps to <color#94c6ff>http://reddit.com/r/cimtographer</color>,\n" +
                            "and show us what you've made!";
            label.textScale = 0.6f;
            label.size = new Vector2(400, 40);
            label.relativePosition = new Vector2(40, height - 32);
            label.processMarkup = true;
            label.eventClicked += BottomLabel_eventClicked;

            UILabel versionLabel = this.AddUIComponent<UILabel>();
            versionLabel.text = "v" + MapperOptionsManager.major.ToString() + "." + MapperOptionsManager.minor.ToString() + "." + MapperOptionsManager.build.ToString();
            versionLabel.textScale = 0.4f;
            versionLabel.size = new Vector2(40, 15);
            versionLabel.relativePosition = new Vector2(360, height - 15);
            versionLabel.processMarkup = true;
            versionLabel.textColor = new Color32(180, 180, 180, 255);
            versionLabel.textAlignment = UIHorizontalAlignment.Right;
            versionLabel.tooltip = versionLabel.text + "." + MapperOptionsManager.revision.ToString();
        }

        private void CreateRightPanel()
        {
            offset = new Vector2(width / 2, titleOffset);

            buttonGenerate = CustomUI.UIUtils.CreateButton(this);
            buttonGenerate.eventClicked += ButtonGenerate_eventClicked;
            buttonGenerate.textPadding = new RectOffset(6, 6, 6, 6);
            buttonGenerate.size = new Vector2(190, 30);
            buttonGenerate.relativePosition = offset;
            buttonGenerate.text = "Generate OSM map";

            offset += new Vector2(0, buttonGenerate.height + 20);

            /*UIButton buttonWhatsNew = CustomUI.UIUtils.CreateButton(this);
            buttonWhatsNew.eventClicked += ButtonWhatsNew_eventClicked;
            buttonWhatsNew.textPadding = new RectOffset(6, 6, 6, 6);
            buttonWhatsNew.size = new Vector2(190, 30);
            buttonWhatsNew.relativePosition = offset;
            buttonWhatsNew.text = "What's new? (18/02/16)";

            offset += new Vector2(0, buttonGenerate.height + 20);*/

            UILabel label = this.AddUIComponent<UILabel>();
            label.text =    "<color#94c6ff>Where's the import gone?</color>\n" +
                            "Sorry, but the import functionality\n" +
                            "is something that needs re-developing,\n" +
                            "and doesn't currently work with the\n" +
                            "new code. Since it was the same\n" +
                            "version as the old Cimtographer mod\n" +
                            "you could still use that mod to\n" +
                            "import for now.\n\n" +

                            "<color#94c6ff>Issues?</color>\n" +
                            "If there's any problems with this\n" +
                            "version of the mod, let me know on\n" +
                            "either Reddit or the Workshop.";
            label.textScale = 0.6f;
            label.size = new Vector2(200, 40);
            label.relativePosition = offset;
            label.processMarkup = true;
        }

        private void CreateOptions()
        {
            scrollOptionsList = UIFastList.Create<UIOptionItem>(this);
            scrollOptionsList.backgroundSprite = "UnlockingPanel";
            scrollOptionsList.size = new Vector2(192, 290);
            scrollOptionsList.canSelect = true;
            scrollOptionsList.relativePosition = offset;
            scrollOptionsList.rowHeight = 20f;
            scrollOptionsList.rowsData.Clear();
            scrollOptionsList.selectedIndex = -1;

            offset += new Vector2(0, scrollOptionsList.height + 5);

            CreateOptionList(MapperOptionsManager.exportOptions, scrollOptionsList);

            scrollOptionsList.DisplayAt(0);
            scrollOptionsList.selectedIndex = 0;
        }

        private void CreateOptionList(Dictionary<string, OptionItem> options, UIFastList list = null)
        {
            if (list != null)
            {
                foreach (KeyValuePair<string, OptionItem> option in options)
                {
                    option.Value.id = option.Key;

                    list.rowsData.Add(option.Value);
                }
            }
        }

        private void ButtonGenerate_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            try
            {
                RoadManager.ClearRoadTypes();

                OSMExportNew osmExporter = new OSMExportNew();
                osmExporter.Export();

                buttonGenerate.text = "Generate OSM map";
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
                buttonGenerate.text = "Export failed!";
            }

            try
            {
                string _osmName = Singleton<SimulationManager>.instance.m_metaData.m_CityName + ".osm";
                string _fullOSMPath = Path.GetFullPath(_osmName);
                string _modPath = CimTools.CimToolsHandler.CimToolBase.Path.GetModPath();

                if (_modPath != null && _modPath != "" && _fullOSMPath != null && _fullOSMPath != "")
                {
                    string _mscriptPath = _modPath + Path.DirectorySeparatorChar + "MaperitiveScript.mscript";
                    string _mrulesPath = _modPath + Path.DirectorySeparatorChar + "Cimtographer.mrules";
                    string _zippedMaperitivePath = _modPath + Path.DirectorySeparatorChar + "Maperitive";
                    string _citiesPath = Path.GetDirectoryName(_fullOSMPath);
                    string _extractedMaperitivePath = _citiesPath + Path.DirectorySeparatorChar + "Maperitive";

                    string _mscript = string.Format(
                        "use-ruleset location=\"{0}\"" +
                        "\r\napply-ruleset" +
                        "\r\nload-source \"{1}\""
                        , _mrulesPath, _fullOSMPath);

                    File.Delete(_mscriptPath);
                    StreamWriter _writer = File.CreateText(_mscriptPath);
                    _writer.Write(_mscript);
                    _writer.Flush();
                    _writer.Close();

                    if (_extractedMaperitivePath != null && _extractedMaperitivePath != "" && GzipExtractor.RemoveAllChangedNames(_zippedMaperitivePath, _extractedMaperitivePath, ".mapper"))
                    {
                        Process _maperitiveProcess = Process.Start(new ProcessStartInfo(
                            _extractedMaperitivePath + Path.DirectorySeparatorChar + "Maperitive",
                            "\"" + _mscriptPath + "\"")
                        { UseShellExecute = false });

                        if(_maperitiveProcess != null)
                        {
                            UIView.library.ShowModal("PauseMenu");
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Couldn't decrypt the gzipped files from " + _zippedMaperitivePath + " to " + _extractedMaperitivePath);
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Could not find OSM at " + _fullOSMPath + " or mod path at " + _modPath);
                }
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
            }
        }

        private void ButtonWhatsNew_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            if(whatsNewPanel != null)
            {
                whatsNewPanel.Show();
                whatsNewPanel.BringToFront();
            }
        }

        private void TopLabel_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            Steam.ActivateGameOverlayToWorkshopItem(new PublishedFileId(549792340));
        }

        private void BottomLabel_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            Steam.ActivateGameOverlayToWebPage("https://www.reddit.com/r/Cimtographer");
        }
    }
}
