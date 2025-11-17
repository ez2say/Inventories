namespace InventorySystem.Contracts
{
    public interface IDropHandler
    {
        bool CanDrop(IDragService dragService, int slotIndex);
        void HandleDrop(IDragService dragService, int slotIndex);
    }
}
