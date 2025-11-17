using InventorySystem.Contracts;
using UnityEngine;

namespace InventorySystem
{
    public class InventorySlot : IInventorySlot
    {
        private readonly int _slotIndex;
        private readonly Vector2Int _position;
        private IInventoryItem _item;
        private int _itemCount;

        public InventorySlot(int slotIndex, int width)
        {
            _slotIndex = slotIndex;
            _position = new Vector2Int(slotIndex % width, slotIndex / width);
            _item = null;
            _itemCount = 0;
        }

        public int SlotIndex => _slotIndex;
        public Vector2Int Position => _position;
        public bool IsEmpty => _item == null;
        public IInventoryItem Item => _item;
        public int ItemCount => _itemCount;

        public bool CanAcceptItem(IInventoryItem item)
        {
            if (item == null) return false;

            if (IsEmpty) return true;

            return _item.CanStackWith(item) && _itemCount < _item.MaxStackSize;
        }

        public void SetItem(IInventoryItem item, int count)
        {
            _item = item;
            _itemCount = Mathf.Clamp(count, 1, item?.MaxStackSize ?? 1);
        }

        public void Clear()
        {
            _item = null;
            _itemCount = 0;
        }

        public void IncreaseCount(int amount)
        {
            if (_item != null)
            {
                _itemCount = Mathf.Min(_itemCount + amount, _item.MaxStackSize);
            }
        }

        public void DecreaseCount(int amount)
        {
            if (_item != null)
            {
                _itemCount = Mathf.Max(_itemCount - amount, 0);
                if (_itemCount == 0)
                {
                    Clear();
                }
            }
        }
    }
}