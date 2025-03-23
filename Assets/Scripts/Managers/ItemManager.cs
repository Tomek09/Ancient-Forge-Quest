using System.Collections.Generic;
using AncientForgeQuest.Models;
using AncientForgeQuest.Utility;
using UnityEngine;

namespace AncientForgeQuest.Managers
{
    public class ItemManager : Singleton<ItemManager>
    {
        [SerializeField] private List<ItemModel> _items;
        
        private Dictionary<int, ItemModel> _itemById = new Dictionary<int, ItemModel>();

        protected override void OnAwake()
        {
            foreach (var item in _items)
            {
                _itemById.Add(item.ItemID, item);
            }
        }

        public bool TryGetItem(int itemId, out ItemModel itemModel)
        {
            return _itemById.TryGetValue(itemId, out itemModel);
        }
    }
}
