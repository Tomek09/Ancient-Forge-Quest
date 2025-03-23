using System;
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
        public Inventory Inventory { get; private set; }

        [Header("Debug")]
        [SerializeField] private ItemModel _itemModel;
        [SerializeField] private int _amount;
        [SerializeField] private bool _addItemModel;
        [SerializeField] private bool _removeItemModel;

        public ReactiveProperty<InventorySlot> SelectedSlot { get; private set; } = new ReactiveProperty<InventorySlot>();
        private IPickable _currentPickable;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
                return;

            if (_addItemModel)
            {
                Inventory.Add(_itemModel, _amount);
                _addItemModel = false;
            }

            if (_removeItemModel)
            {
                //Inventory.Remove(_itemModel, _amount);
                _removeItemModel = false;
            }
        }
        #endif

        protected override void OnAwake()
        {
            Inventory = new Inventory(3);
            Inventory.Add(_itemModel, 5);
        }

        private void Update()
        {
            PrintInventory();
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
        
        private void PrintInventory()
        {
            int index = 0;
            foreach (var slot in Inventory.Slots)
            {
                var output = slot.IsEmpty() ? $"{index}. Empty" : $"{index}. {slot.Item.CurrentValue.Name}, {slot.GetAmountText()}";
                Debug.Log(output);
                index++;
            }
        }
    }
}
