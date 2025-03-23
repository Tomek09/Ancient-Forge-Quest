using System.Collections.Generic;
using System.Linq;
using AncientForgeQuest.Inventories;
using AncientForgeQuest.Utility;
using UnityEngine;

namespace AncientForgeQuest.Models
{
    [CreateAssetMenu(fileName = "New Loot Table", menuName = "Models/Loot Table")]
    public class LootTableModel : ScriptableObject
    {
        [System.Serializable]
        private class LootTable
        {
            public ItemModel Item;
            public Vector2Int AmountRange;
            [Range(0, 1)] public float ChanceRate;
        }

        [Header("Properties")]
        [SerializeField] private LootTable[] _lootTables;

        public List<ItemBag> GetItemBags()
        {
            return _lootTables
                .Where(x => x.ChanceRate.IsSuccess())
                .Select(x => new ItemBag(x.Item, x.AmountRange.GetRandom()))
                .ToList();
        }
    }
}
