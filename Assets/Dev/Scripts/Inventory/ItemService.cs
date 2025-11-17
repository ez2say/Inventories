using InventorySystem.Contracts;
using UnityEngine;
using Zenject;

namespace InventorySystem
{
    public class ItemService : IItemService
    {
        private readonly ItemDatabase _itemDatabase;

        public ItemDatabase ItemDatabase => _itemDatabase;

        public ItemService(ItemDatabase itemDatabase)
        {
            _itemDatabase = itemDatabase;
        }

        public IInventoryItem CreateItem(string itemId, int stackSize = 1)
        {
            var config = _itemDatabase.GetItemConfigById(itemId);
            if (config == null)
            {
                Debug.LogError($"Item config not found: {itemId}");
                return null;
            }

            return new InventoryItem(config, stackSize);
        }

        public bool ItemExists(string itemId)
        {
            return _itemDatabase.GetItemConfigById(itemId) != null;
        }
    }
}