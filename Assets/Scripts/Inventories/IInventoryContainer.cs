using AncientForgeQuest.Models;

namespace AncientForgeQuest.Inventories
{
    public interface IInventoryContainer
    {
        int Add(ItemModel item, int amount);
        
        //void Remove(InventorySlot slot);
    }
}
