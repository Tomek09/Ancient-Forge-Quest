using AncientForgeQuest.Models;

namespace AncientForgeQuest.Inventories
{
    public interface IInventoryContainer
    {
        int Add(ItemModel item, int amount);
        
        int Add(ItemBag itemBag);

        //void Remove(InventorySlot slot);
    }
}
