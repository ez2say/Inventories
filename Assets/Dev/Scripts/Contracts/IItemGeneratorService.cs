using System.Collections.Generic;

namespace InventorySystem.Contracts
{
    public interface IItemGeneratorService
    {
        IInventoryItem GenerateRandomItem();
        List<IInventoryItem> GenerateInitialInventory(int maxItems);
    }
}