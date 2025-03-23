using System;
using System.Collections.Generic;
using System.Linq;
using AncientForgeQuest.Inventories;
using AncientForgeQuest.Models;
using AncientForgeQuest.Utility;
using R3;
using UnityEngine;

namespace AncientForgeQuest.Managers
{
    public class BonusesManager : Singleton<BonusesManager>, IDisposable
    {
        private InventorySlot[] _playerSlots;

        private Dictionary<InventorySlot, ItemModel> _itemBySlot = new Dictionary<InventorySlot, ItemModel>();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private void Start()
        {
            var inventory = GameInventoryManager.Instance.Inventory;
            foreach (var slot in inventory.Slots)
            {
                _itemBySlot.Add(slot, slot.Item.CurrentValue);
                slot.OnInventorySlotChanged.Subscribe(_ => OnInventorySlotChanged(slot)).AddTo(_disposables);
            }
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }

        public bool TryGetBonus(BonusType bonusType, out float value)
        {
            value = -1;
            foreach (var item in _itemBySlot.Values.Where(item => item != null))
            {
                if (item is not BonusItem bonusItem)
                    continue;

                if (bonusItem.BonusType != bonusType)
                    continue;

                value = bonusItem.Value;
                return true;
            }

            return false;
        }

        private void OnInventorySlotChanged(InventorySlot slot)
        {
            _itemBySlot[slot] = slot.Item.CurrentValue;
        }
    }
}
