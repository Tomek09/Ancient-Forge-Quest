using System.Collections.Generic;
using AncientForgeQuest.Inventories;
using AncientForgeQuest.UI.DesignSystem;
using UnityEngine;

namespace AncientForgeQuest.UI.Inventories
{
    public abstract class InventoryView : BindableBehaviour<Inventory>
    {
        [Header("Components")]
        [SerializeField] private InventorySlotView _slotPrefab;
        [SerializeField] private Transform _slotParent;

        protected Dictionary<InventorySlot, InventorySlotView> _slotByView = new Dictionary<InventorySlot, InventorySlotView>();

        protected void CreateSlot(InventorySlot slot)
        {
            if (_slotByView.ContainsKey(slot))
            {
                Debug.LogError($"Slot already exists! ({slot})");
                return;
            }
            
            var slotView = Instantiate(_slotPrefab, _slotParent);
            slotView.Bind(slot);
            _slotByView.Add(slot, slotView);
        }
    }
}
