using AncientForgeQuest.Models;
using R3;
using UnityEngine;

namespace AncientForgeQuest.Inventories
{
    [System.Serializable]
    public class InventorySlot
    {
        private const string AmountFormat = "x {0}";

        public ReactiveProperty<ItemModel> Item = new ReactiveProperty<ItemModel>();
        public ReactiveProperty<int> Amount = new ReactiveProperty<int>();
        public Inventory Inventory { get; private set; }

        public InventorySlot(Inventory inventory)
        {
            Inventory = inventory; 
            Clear(); 
        }
        
        public InventorySlot(Inventory inventory, ItemModel itemModel, int amount)
        {
            Inventory = inventory;
            Bind(itemModel, amount);
        }
        
        public void Bind(ItemModel item, int amount)
        {
            Item.Value = item;
            Amount.Value = amount;
        }
        
        public void Bind(InventorySlot slot)
        {
            Item.Value = slot.Item.CurrentValue;
            Amount.Value = slot.Amount.CurrentValue;
        }

        public void Clear()
        {
            Item.Value = null;
            Amount.Value = 0;
        }

        public void Add(int value)
        {
            SetAmount(Amount.CurrentValue + Mathf.Abs(value));
        }
        
        public void Remove(int value)
        {
            SetAmount(Amount.CurrentValue - Mathf.Abs(value));
        }

        public void SetAmount(int value)
        {
            Amount.Value = value;
        }
        

        public bool IsEmpty()
        {
            return Item.CurrentValue == null;
        }

        public bool IsFull()
        {
            return Item.CurrentValue != null && Amount.CurrentValue >= Item.CurrentValue.MaxSize;
        }

        public bool HasItem(InventorySlot slot)
        {
            return Item.CurrentValue == slot.Item.CurrentValue;
        }

        public string GetAmountText()
        {
            return Amount.CurrentValue <= 0 ? string.Empty : string.Format(AmountFormat, Amount);
        }
    }
}
