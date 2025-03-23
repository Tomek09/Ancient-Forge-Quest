using AncientForgeQuest.Inventories;
using AncientForgeQuest.Inventories.Interfaces;
using AncientForgeQuest.Models;
using AncientForgeQuest.Utility;
using R3;
using UnityEngine;

namespace AncientForgeQuest.Managers
{
    public class GameInventoryManager : Singleton<GameInventoryManager>
    {
        [Header("Components")]
        [SerializeField] private LootTableModel _startInventory;

        public Inventory Inventory { get; private set; }

        public ReactiveProperty<InventorySlot> SelectedSlot { get; private set; } = new ReactiveProperty<InventorySlot>();
        private IPickable _currentPickable;

        protected override void OnAwake()
        {
            Inventory = new Inventory(27);
            var bags = _startInventory.GetItemBags();
            Inventory.Add(bags);
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            if (_currentPickable == null)
            {
                HandlePickup();
            }
            else
            {
                HandleDrop();
            }
        }

        private void HandlePickup()
        {
            if (!UIExtension.TryGetUIComponent(out IPickable pickable))
                return;

            if (!pickable.IsPickable())
                return;

            ResetPickable();

            _currentPickable = pickable;
            SelectedSlot.Value = pickable.Model;
            pickable.OnPickupStart();
        }

        private void HandleDrop()
        {
            if (UIExtension.TryGetUIComponent(out IDroppable droppable))
            {
                var remainingAmount = droppable.OnDrop(_currentPickable.Model);

                if (remainingAmount > 0)
                    return;
            }

            ResetPickable();
        }

        private void ResetPickable()
        {
            if (_currentPickable == null)
                return;

            _currentPickable.OnPickupEnd();
            _currentPickable = null;
            SelectedSlot.Value = null;
        }
    }
}
