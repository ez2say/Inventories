using InventorySystem.Contracts;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : IInventory
    {
        private readonly int _width;
        private readonly int _height;
        private readonly IInventorySlot[] _slots;

        public Inventory(int width, int height)
        {
            _width = width;
            _height = height;
            _slots = new IInventorySlot[width * height];

            InitializeSlots();
        }

        public int Width => _width;
        public int Height => _height;
        public int TotalSlots => _slots.Length;

        private void InitializeSlots()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i] = new InventorySlot(i, _width);
            }
        }

        public IInventorySlot GetSlot(int index)
        {
            if (index >= 0 && index < _slots.Length)
            {
                return _slots[index];
            }
            return null;
        }

        public IInventorySlot GetSlot(int x, int y)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                int index = y * _width + x;
                return GetSlot(index);
            }
            return null;
        }

        public bool TryAddItem(IInventoryItem item, int count)
        {
            if (item == null || count <= 0) return false;

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].IsEmpty)
                {
                    _slots[i].SetItem(item, count);
                    return true;
                }
            }

            return false;
        }

        public bool TryRemoveItem(string itemId, int count)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (!_slots[i].IsEmpty && _slots[i].Item.ItemId == itemId)
                {
                    _slots[i].Clear();
                    return true;
                }
            }

            return false;
        }

        public void SwapSlots(int slotIndexA, int slotIndexB)
        {
            if (slotIndexA < 0 || slotIndexA >= _slots.Length ||
                slotIndexB < 0 || slotIndexB >= _slots.Length) return;

            var slotA = _slots[slotIndexA];
            var slotB = _slots[slotIndexB];

            var tempItem = slotA.Item;
            var tempCount = slotA.ItemCount;

            slotA.SetItem(slotB.Item, slotB.ItemCount);
            slotB.SetItem(tempItem, tempCount);
        }

        public void ClearSlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < _slots.Length)
            {
                _slots[slotIndex].Clear();
            }
        }
    }
}