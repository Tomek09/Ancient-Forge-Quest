using System;
using AncientForgeQuest.Managers;

namespace AncientForgeQuest.UI.Inventories
{
    public class GameInventoryView : InventoryView
    {
        private void Start()
        {
            var inventory = GameInventoryManager.Instance.Inventory;
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
