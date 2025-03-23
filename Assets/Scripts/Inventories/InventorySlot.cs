using AncientForgeQuest.Models;
using UnityEngine;

namespace AncientForgeQuest.Inventories
{
    public class InventorySlot
    {
        private const string AmountFormat = "x {0}";
        
        public ItemModel Item;
        public int Amount;

        public InventorySlot() => Clear();
        
        public InventorySlot(ItemModel item, int amount) => Bind(item, amount);
        
        public void Bind(ItemModel item, int amount)
        {
            Item = item;
            Amount = amount;
        }
        
        public void Clear()
        {
            Item = null;
            Amount = 0;
        }

        public void Add(int value)
        {
            Amount += Mathf.Abs(value);
        }

        public bool IsEmpty()
        {
            return Item == null;
        }
        
        public string GetAmountText()
        {
            return Amount <= 0 ? string.Empty : string.Format(AmountFormat, Amount);
        }
    }
}
