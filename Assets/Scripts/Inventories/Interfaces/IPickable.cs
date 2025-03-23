namespace AncientForgeQuest.Inventories.Interfaces
{
    public interface IPickable
    {
        public InventorySlot Model { get; }
        
        void OnPickupStart();
        
        void OnPickupEnd();
        
        bool IsPickable();
    }
}
