using System;
using AncientForgeQuest.Inventories;
using AncientForgeQuest.UI.Inventories;
namespace AncientForgeQuest.UI.Machines
{
    public class MachineInventoryView : InventoryView
    {
        private void Start()
        {
            var inventory = new Inventory(1);
            Bind(inventory);
        }

        protected override void OnBind()
        {
            foreach (var slot in Model.Slots)
            {
                CreateSlot(slot);
            }
        }
    }
}
