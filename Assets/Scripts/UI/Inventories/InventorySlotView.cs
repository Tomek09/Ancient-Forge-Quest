using AncientForgeQuest.Inventories;
using AncientForgeQuest.Models;
using AncientForgeQuest.UI.DesignSystem;
using R3;
using TMPro;
using UnityEngine;

namespace AncientForgeQuest.UI.Inventories
{
    public class InventorySlotView : BindableBehaviour<InventorySlot>
    {
        [Header("Components")]
        [SerializeField] private ItemView _itemView;
        [SerializeField] private TMP_Text _amountText;

        protected override void OnBind()
        {
            if (Model == null)
                return;

            Model.Item.Subscribe(OnItemChange).AddTo(_disposables);
            Model.Amount.Subscribe(OnAmountChange).AddTo(_disposables);
        }

        private void OnItemChange(ItemModel itemModel)
        {
            _itemView.Bind(itemModel);
        }

        private void OnAmountChange(int amount)
        {
            _amountText.SetText(Model.GetAmountText());
        }
    }
}
