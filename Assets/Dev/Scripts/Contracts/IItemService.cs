namespace InventorySystem.Contracts
{
    public interface IItemService
    {
        ItemDatabase ItemDatabase { get; }
        IInventoryItem CreateItem(string itemId, int stackSize = 1);
        bool ItemExists(string itemId);
    }
}