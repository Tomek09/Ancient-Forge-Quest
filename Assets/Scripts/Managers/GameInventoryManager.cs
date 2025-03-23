using AncientForgeQuest.Inventories;
using AncientForgeQuest.Models;
using AncientForgeQuest.Utility;
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
            Inventory = new Inventory(25);
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
