using AncientForgeQuest.Inventories;
using AncientForgeQuest.Inventories.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AncientForgeQuest.UI.Inventories
{
    [RequireComponent(typeof(InventorySlotView))]
    public class DropController : MonoBehaviour, IDroppable
    {
        private InventorySlotView _slotView;
        private InventorySlot Model => _slotView.Model;

        private void Awake()
        {
            _slotView = GetComponent<InventorySlotView>();
        }
        
        public int OnDrop(InventorySlot slot)
        {
            if (slot == Model)
                return 0;

            Debug.Log(Keyboard.current.ctrlKey.ReadValue());
            
            if (Model.IsEmpty())
            {
                Model.Bind(slot);
                slot.Clear();
                return 0;
            }

            if (Model.HasItem(slot))
            {
                var amount = slot.Amount.CurrentValue;
                var remainingAmount = Model.Inventory.Add(Model, slot.Amount.CurrentValue);
                slot.Inventory.Remove(slot, amount - remainingAmount);

                return remainingAmount;
            }
            
            
            var tempItem = slot.Item.CurrentValue;
            var tempAmount = slot.Amount.CurrentValue;
            slot.Bind(Model);
            Model.Bind(tempItem, tempAmount);
            
            return 0;
        }
    }
}
