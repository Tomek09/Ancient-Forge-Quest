using AncientForgeQuest.Inventories;
using AncientForgeQuest.Models;
using AncientForgeQuest.UI.DesignSystem;
using AncientForgeQuest.Utility;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AncientForgeQuest.UI.Inventories
{
    public class DraggableSlotInventoryView : BindableBehaviour<InventorySlot>
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ItemView _itemView;
        [SerializeField] private TMP_Text _amountText;
        
        private void LateUpdate()
        {
            if (Model == null)
                return;
            
            transform.position = Mouse.current.position.ReadValue();
        }
        
        protected override void OnBind()
        {
            if (Model == null)
            {
                Toggle(false);
                return;
            }

            Model.Item.Subscribe(OnItemChange).AddTo(_disposables);
            Model.Amount.Subscribe(OnAmountChange).AddTo(_disposables);
            Toggle(true);
        }
        
        private void OnItemChange(ItemModel itemModel)
        {
            _itemView.Bind(itemModel);
        }
        
        private void OnAmountChange(int amount)
        {
            _amountText.SetText(Model.GetAmountText());
        }
        
        
        private void Toggle(bool value)
        {
            _canvasGroup.Toggle(value);
        }
    }
}
