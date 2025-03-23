using System.Collections.Generic;
using System.Linq;
using AncientForgeQuest.Models;
using UnityEngine;

namespace AncientForgeQuest.Inventories
{
    public class Inventory : IInventoryContainer
    {
        private readonly InventorySlot[] _slots;
        private readonly int _capacity;

        public event System.Action<InventorySlot> OnItemUpdate = delegate { };

        public Inventory(int capacity)
        {
            _capacity = capacity;
            _slots = new InventorySlot[_capacity];
            for (int i = 0; i < _capacity; i++)
            {
                _slots[i] = new InventorySlot();
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
                    var space = maxSize - currentItemSlot.Amount;
                    var value = Mathf.Min(remainingAmount, space);
                    remainingAmount -= value;

                    currentItemSlot.Add(value);
                    OnItemUpdate?.Invoke(currentItemSlot);

                    if (remainingAmount <= 0)
                        break;
                }
            }

            if (remainingAmount > 0 && TryGetEmptySlots(out var emptySlots))
            {
                foreach (var emptySlot in emptySlots)
                {
                    int space = Mathf.Min(remainingAmount, maxSize);
                    remainingAmount -= space;

                    emptySlot.Bind(item, space);
                    OnItemUpdate?.Invoke(emptySlot);

                    if (remainingAmount <= 0)
                        break;
                }
            }
            
            return remainingAmount;
        }
        
        public int Add(ItemBag itemBag)
        {
            return Add(itemBag.Item, itemBag.Amount);
        }

        
        public void Swap(int from, int to)
        {
            Debug.Log("TODO");
        }


        private bool TryGetItemSlots(ItemModel item, out List<InventorySlot> slots)
        {
            return TryGetSlots(x => x.Item == item && x.Amount < item.MaxSize, out slots);
        }

        private bool TryGetEmptySlots(out List<InventorySlot> slots)
        {
            return TryGetSlots(x => x.IsEmpty(), out slots);
        }

        private bool TryGetSlots(System.Func<InventorySlot, bool> predicate, out List<InventorySlot> slots)
        {
            slots = _slots
                .Where(predicate.Invoke)
                .ToList();

            return slots.Count > 0;
        }
    }
}
