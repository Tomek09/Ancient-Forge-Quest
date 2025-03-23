using System.Collections.Generic;
using System.Linq;
using AncientForgeQuest.Models;
using UnityEngine;

namespace AncientForgeQuest.Inventories
{
    public class Inventory : IInventoryContainer
    {
        public InventorySlot[] Slots { get; private set; }
        public int Capacity { get; private set; }

        public Inventory(int capacity)
        {
            Capacity = capacity;
            Slots = new InventorySlot[Capacity];
            for (int i = 0; i < Capacity; i++)
            {
                var slot = new InventorySlot(this);
                Slots[i] = slot;
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
                    remainingAmount = Add(currentItemSlot, remainingAmount);
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

        public void Add(ItemBag itemBag)
        {
            Add(itemBag.Item, itemBag.Amount);
        }

        public void Add(List<ItemBag> itemBags)
        {
            foreach (var bag in itemBags)
            {
                Add(bag);
            }
        }

        public int Add(InventorySlot slot, int amount)
        {
            var maxSize = slot.Item.CurrentValue.MaxSize;

            var space = maxSize - slot.Amount.CurrentValue;
            var value = Mathf.Min(amount, space);
            slot.Add(value);

            return amount - value;
        }


        public int Remove(ItemModel item, int amount)
        {
            var remainingAmount = amount;

            if (!TryGetItemSlots(item, out var currentItemSlots))
                return remainingAmount;

            foreach (var currentItemSlot in currentItemSlots)
            {
                remainingAmount = Remove(currentItemSlot, remainingAmount);
                if (remainingAmount <= 0)
                    break;
            }

            return remainingAmount;
        }

        public int Remove(InventorySlot slot, int amount)
        {
            var value = Mathf.Min(amount, slot.Amount.CurrentValue);
            slot.Remove(value);
            if (slot.Amount.CurrentValue <= 0)
            {
                slot.Clear();
            }

            return amount - value;
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
