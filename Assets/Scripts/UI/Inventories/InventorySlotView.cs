using AncientForgeQuest.Instances;
using AncientForgeQuest.Inventories;
using AncientForgeQuest.Models;
using AncientForgeQuest.UI.DesignSystem;
using AncientForgeQuest.UI.Tooltips;
using R3;
using TMPro;
using UnityEngine;

namespace AncientForgeQuest.UI.Inventories
{
    public class InventorySlotView : BindableBehaviour<InventorySlot>, ITooltipProvider
    {
        [Header("Components")]
        [SerializeField] private ItemView _itemView;
        [SerializeField] private TMP_Text _amountText;
        private TooltipContent _tooltipContent;

        protected override void OnBind()
        {
            if (Model == null)
            {
                _tooltipContent = new TooltipContent();
                return;
            }

            Model.Item.Subscribe(OnItemChange).AddTo(_disposables);
            Model.Amount.Subscribe(OnAmountChange).AddTo(_disposables);
        }

        private void OnItemChange(ItemModel itemModel)
        {
            UpdateTooltip(itemModel);
            _itemView.Bind(itemModel);
        }

        private void OnAmountChange(int amount)
        {
            _amountText.SetText(Model.GetAmountText());
        }

        private void UpdateTooltip(ItemModel itemModel)
        {
            _tooltipContent = itemModel == null 
                ? new TooltipContent() 
                : new TooltipContent(itemModel.Name, itemModel.GetDescription());
        }

        public TooltipContent GetContent() => _tooltipContent;
    }
}
