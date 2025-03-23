using AncientForgeQuest.Models;
namespace AncientForgeQuest.Inventories
{
    [System.Serializable]
    public struct ItemBag
    {
        public ItemModel Item;
        public int Amount;

        public ItemBag(ItemModel item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
}
