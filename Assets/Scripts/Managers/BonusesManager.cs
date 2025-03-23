using AncientForgeQuest.Inventories;
using AncientForgeQuest.Models;
using AncientForgeQuest.Utility;

namespace AncientForgeQuest.Managers
{
    public class BonusesManager : Singleton<BonusesManager>
    {
        private InventorySlot[] _playerSlots;
        
        private void Start()
        {
            var inventory = GameInventoryManager.Instance.Inventory;
            _playerSlots = inventory.Slots;
        }

        public bool TryGetBonus(BonusType bonusType, out float value)
        {
            // TODO: Big no no no no
            
            value = -1;
            foreach (var slot in _playerSlots)
            {
                if (slot.IsEmpty())
                    continue;

                if (slot.Item.CurrentValue is not BonusItem bonusItem)
                    continue;

                if (bonusItem.BonusType != bonusType)
                    continue;
                
                value = bonusItem.Value;
            }

            return false;
        }
    }
}
