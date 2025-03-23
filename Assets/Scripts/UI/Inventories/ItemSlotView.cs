using AncientForgeQuest.Inventories;
using AncientForgeQuest.UI.DesignSystem;
using TMPro;
using UnityEngine;

namespace AncientForgeQuest.UI.Inventories
{
    public class ItemSlotView : BindableBehaviour<InventorySlot>
    {
        [Header("Components")]
        [SerializeField] private ItemView _itemView;
        [SerializeField] private TMP_Text _amountText;
        
        protected override void OnBind()
        {
            // TODO R3;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (Model == null)
            {
                _itemView.Bind(Model.Item);
                _amountText.SetText(Model.GetAmountText());
            }
        }
    }
}
