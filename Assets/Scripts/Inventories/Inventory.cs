using System.Collections.Generic;
using System.Linq;
using AncientForgeQuest.Models;
using UnityEngine;

namespace AncientForgeQuest.Inventories
{
    public class Inventory : IInventoryContainer
    {
        public InventorySlot[] Slots { get; private set; }
        private readonly int _capacity;
        
        public Inventory(int capacity)
        {
            _capacity = capacity;
            Slots = new InventorySlot[_capacity];
            for (int i = 0; i < _capacity; i++)
            {
                Slots[i] = new InventorySlot(this);
            }
        }

        public int Add(ItemModel item, int amount)
        {
            var remainingAmount = amount;
            var maxSize = item.MaxSize;

            if (TryGetItemSlots(item, out var currentItemSlots))
            {
                foreach (var currentItemSlot in currentItemSlots)
                {
                    remainingAmount = AddTo(currentItemSlot, remainingAmount);
                    if (remainingAmount <= 0)
                        break;
                }
            }

            if (remainingAmount <= 0 || !TryGetEmptySlots(out var emptySlots))
                return remainingAmount;
            
            foreach (var emptySlot in emptySlots)
            {
                int space = Mathf.Min(remainingAmount, maxSize);
                remainingAmount -= space;

                emptySlot.Bind(item, space);

                if (remainingAmount <= 0)
                    break;
            }
            
            return remainingAmount;
        }

        public int AddTo(InventorySlot slot, int amount)
        {
            var maxSize = slot.Item.CurrentValue.MaxSize;
            
            var space = maxSize - slot.Amount.CurrentValue;
            var value = Mathf.Min(amount, space);
            slot.Add(value);

            return amount - value;
        }
        
        public int AddTo(InventorySlot slot, InventorySlot from)
        {
            return AddTo(slot, from.Amount.CurrentValue);
        }
        
        public int Add(ItemBag itemBag)
        {
            return Add(itemBag.Item, itemBag.Amount);
        }
        
        private bool TryGetItemSlots(ItemModel item, out List<InventorySlot> slots)
        {
            return TryGetSlots(x => x.Item.CurrentValue == item && x.Amount.CurrentValue < item.MaxSize, out slots);
        }

        private bool TryGetEmptySlots(out List<InventorySlot> slots)
        {
            return TryGetSlots(x => x.IsEmpty(), out slots);
        }

        private bool TryGetSlots(System.Func<InventorySlot, bool> predicate, out List<InventorySlot> slots)
        {
            slots = Slots
                .Where(predicate.Invoke)
                .ToList();

            return slots.Count > 0;
        }
    }
}
