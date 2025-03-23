using AncientForgeQuest.UI.Inventories;
using AncientForgeQuest.Utility;
using UnityEngine;

namespace AncientForgeQuest.Managers
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("Components")]
        [SerializeField] private DraggableSlotInventoryView _draggableItemView;

        private InventorySlotView _currentSlotView = null;

        private void Start()
        {
            ClearCurrentSlot();
        }

        public void OnInventorySlotSelected(InventorySlotView slotView)
        {
            if (_currentSlotView == null)
            {
                if (slotView.Model.IsEmpty())
                    return;

                SetCurrentSlot(slotView);
                return;
            }

            if (slotView == _currentSlotView)
            {
                ClearCurrentSlot();
                return;
            }

            if (slotView.Model.IsEmpty())
            {
                slotView.Model.Bind(_currentSlotView.Model);
                _currentSlotView.Model.Clear();

                ClearCurrentSlot();
                return;
            }

            if (slotView.Model.HasItem(_currentSlotView.Model))
            {
                if (slotView.Model.IsFull())
                    return;

                int remainingAmount = slotView.Model.Inventory.AddTo(slotView.Model, _currentSlotView.Model);

                if (remainingAmount <= 0)
                {
                    ClearCurrentSlot();
                    return;
                }
                
                _currentSlotView.Model.SetAmount(remainingAmount);
                return;
            }
            
            var tempItem = slotView.Model.Item.CurrentValue;
            var tempAmount = slotView.Model.Amount.CurrentValue;
            slotView.Model.Bind(_currentSlotView.Model);
            _currentSlotView.Model.Bind(tempItem, tempAmount);
        }

        private void SetCurrentSlot(InventorySlotView slotView)
        {
            _currentSlotView = slotView;
            _currentSlotView.Toggle(false);
            _draggableItemView.Bind(slotView.Model);
        }
        
        private void ClearCurrentSlot()
        {
            _currentSlotView?.Toggle(true);
            _currentSlotView = null;
            _draggableItemView.Bind(null);
        }
    }
}
