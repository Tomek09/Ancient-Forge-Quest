using AncientForgeQuest.Instances;
using AncientForgeQuest.Models;
using AncientForgeQuest.UI.DesignSystem;
using AncientForgeQuest.UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace AncientForgeQuest.UI.Inventories
{
    public class ItemView : BindableBehaviour<ItemModel>, ITooltipProvider
    {
        [Header("Components")]
        [SerializeField] private Image _icon;
        private TooltipContent _tooltipContent;

        protected override void OnBind()
        {
            if (Model == null)
            {
                _tooltipContent = new TooltipContent();
                ToggleIcon(false);
                return;
            }

            ToggleIcon(true);
            _icon.sprite = Model.Icon;

            _tooltipContent = new TooltipContent(Model.Name, Model.Description);
        }

        private void ToggleIcon(bool value)
        {
            _icon.enabled = value;
        }

        public TooltipContent GetContent() => _tooltipContent;
    }
}
