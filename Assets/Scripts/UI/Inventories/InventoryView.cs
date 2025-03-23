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

        protected void CreateSlot(InventorySlot slot)
        {
            var slotView = Instantiate(_slotPrefab, _slotParent);
            slotView.Bind(slot);
        }
    }
}
