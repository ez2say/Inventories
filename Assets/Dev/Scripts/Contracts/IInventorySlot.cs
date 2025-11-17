using UnityEngine;

namespace InventorySystem.Contracts
{
    public interface IInventorySlot
    {
        int SlotIndex { get; }
        Vector2Int Position { get; }
        bool IsEmpty { get; }
        IInventoryItem Item { get; }
        int ItemCount { get; }

        bool CanAcceptItem(IInventoryItem item);
        void SetItem(IInventoryItem item, int count);
        void Clear();
        void IncreaseCount(int amount);
        void DecreaseCount(int amount);
    }
}