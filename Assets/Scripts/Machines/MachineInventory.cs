using System.Collections.Generic;
using AncientForgeQuest.Inventories;
namespace AncientForgeQuest.Machines
{
    public class MachineInventory : Inventory
    {
        public InventorySlot[] InputSlots { get; private set; }

        public InventorySlot OutputSlot { get; private set; }

        public MachineInventory(int capacity) : base(capacity)
        {
            InputSlots = new InventorySlot[Capacity - 1];
            for (int i = 0; i < Capacity - 1; i++)
            {
                InputSlots[i] = Slots[i];
            }
            OutputSlot = Slots[Capacity - 1];
        }
    }
}
