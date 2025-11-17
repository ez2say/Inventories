namespace InventorySystem.Contracts
{
    public interface IInventoryGenerator
    {
        void GenerateInitialInventory(IInventory inventory);
    }
}