namespace InventorySystem.Contracts
{
    public interface IInventoryService
    {
        IInventory PlayerInventory { get; }
        void Initialize();
        bool TryRemoveItemFromSlot(int slotIndex);
        bool TryAddItemToSlot(int slotIndex, string itemId, int count = 1);
        void RegenerateInventory();
    }
}