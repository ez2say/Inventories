using UnityEngine;

namespace InventorySystem.Contracts
{
    public interface IInventoryItem
    {
        string ItemId { get; }
        string ItemName { get; }
        Sprite Icon { get; }
        ItemType ItemType { get; }
        int CurrentStackSize { get; }
        int MaxStackSize { get; }
        bool IsStackable { get; }

        bool CanStackWith(IInventoryItem other);
        int GetAvailableSpaceForStack();
        IInventoryItem CloneWithStackSize(int newStackSize);
    }
}