namespace InventorySystem.Contracts
{
    public interface IInventory
    {
        int Width { get; }
        int Height { get; }
        int TotalSlots { get; }

        IInventorySlot GetSlot(int index);
        IInventorySlot GetSlot(int x, int y);

        bool TryAddItem(IInventoryItem item, int count);
        bool TryRemoveItem(string itemId, int count);
        void SwapSlots(int slotIndexA, int slotIndexB);
        void ClearSlot(int slotIndex);
    }
}