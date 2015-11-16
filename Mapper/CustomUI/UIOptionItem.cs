using UnityEngine;
using ColossalFramework.UI;
using Mapper.Managers;

namespace Mapper.CustomUI
{
    class UIOptionItem : UIPanel, IUIFastListRow
    {
        private UIPanel background;
        private UICheckBox checkboxOption;

        public override void Start()
        {
            base.Start();

            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = parent.width;
            height = 40;

            background = AddUIComponent<UIPanel>();
            background.width = width;
            background.height = 40;
            background.relativePosition = Vector2.zero;
            background.zOrder = 0;

            checkboxOption = UIUtils.CreateCheckBox(this);
            checkboxOption.eventCheckChanged += CheckboxOption_eventCheckChanged;
            checkboxOption.relativePosition = Vector2.zero;
        }

        protected override void OnClick(UIMouseEventParameter p)
        {
            base.OnClick(p);

            //m_name.textColor = new Color32(255, 255, 255, 255);
        }

        public void Display(object data, bool isRowOdd)
        {
            OptionItem option = data as OptionItem;

            if (option != null)
            {
                
                checkboxOption.name = option.id;
                checkboxOption.text = option.readableLabel;
                checkboxOption.label.text = option.readableLabel + (option.enabled ? "" : " (x)");
                checkboxOption.isChecked = option.value;
                checkboxOption.isEnabled = option.enabled;
                checkboxOption.width = this.width;
                checkboxOption.height = 20;

                if (isRowOdd)
                {
                    background.backgroundSprite = "UnlockingItemBackground";
                    background.color = new Color32(0, 0, 0, 128);
                }
                else
                {
                    background.backgroundSprite = null;
                }
            }
        }

        private void CheckboxOption_eventCheckChanged(UIComponent component, bool value)
        {
            UICheckBox checkbox = component as UICheckBox;

            if (MapperOptionsManager.exportOptions.ContainsKey(checkbox.name))
            {
                MapperOptionsManager.exportOptions[checkbox.name].value = value;
                //Debug.Log("Set \"" + checkbox.name + "\" to " + value.ToString());
            }
            else
            {
                Debug.LogError("Could not find option \"" + checkbox.name + "\".");
            }
        }

        public void Select(bool isRowOdd)
        {
            //m_name.textColor = new Color32(255, 255, 255, 255);

            background.backgroundSprite = "ListItemHighlight";
            background.color = new Color32(255, 255, 255, 255);
        }

        public void Deselect(bool isRowOdd)
        {
            //m_name.textColor = options.enabled ? new Color32(255, 255, 255, 255) : new Color32(128, 128, 128, 255);

            if (isRowOdd)
            {
                background.backgroundSprite = "UnlockingItemBackground";
                background.color = new Color32(0, 0, 0, 128);
            }
            else
            {
                background.backgroundSprite = null;
            }
        }

    }
}
