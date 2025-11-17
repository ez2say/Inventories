using InventorySystem.Contracts;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryItem : IInventoryItem
    {
        private readonly ItemConfig _config;
        private int _currentStackSize;

        public InventoryItem(ItemConfig config, int stackSize = 1)
        {
            _config = config;
            _currentStackSize = Mathf.Clamp(stackSize, 1, config.MaxStackSize);
        }

        public string ItemId => _config.ItemId;
        public string ItemName => _config.ItemName;
        public Sprite Icon => _config.Icon;
        public ItemType ItemType => _config.ItemType;
        public int CurrentStackSize => _currentStackSize;
        public int MaxStackSize => _config.MaxStackSize;
        public bool IsStackable => _config.IsStackable;

        public bool CanStackWith(IInventoryItem other)
        {
            if (other == null) return false;
            if (!IsStackable || !other.IsStackable) return false;
            return ItemId == other.ItemId;
        }

        public int GetAvailableSpaceForStack()
        {
            return MaxStackSize - CurrentStackSize;
        }

        public void SetStackSize(int newSize)
        {
            _currentStackSize = Mathf.Clamp(newSize, 1, MaxStackSize);
        }
        public IInventoryItem CloneWithStackSize(int newStackSize)
        {
            return new InventoryItem(_config, newStackSize);
        }
    }
}