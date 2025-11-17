using InventorySystem.Contracts;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace InventorySystem
{

    public class ItemGeneratorService : IItemGeneratorService
    {
        private readonly ItemDatabase _itemDatabase;
        private readonly System.Random _random;

        public ItemGeneratorService(ItemDatabase itemDatabase)
        {
            _itemDatabase = itemDatabase;
            _random = new System.Random();
        }

        public IInventoryItem GenerateRandomItem()
        {
            var allConfigs = _itemDatabase.GetAllItemConfigs();
            if (allConfigs.Count == 0) return null;

            var randomConfig = allConfigs[_random.Next(allConfigs.Count)];
            int stackSize = 1;

            if (randomConfig.IsStackable)
            {
                stackSize = _random.Next(1, randomConfig.MaxStackSize + 1);
            }

            return new InventoryItem(randomConfig, stackSize);
        }

        public List<IInventoryItem> GenerateInitialInventory(int maxItems)
        {
            var items = new List<IInventoryItem>();
            int itemCount = _random.Next(1, maxItems + 1);

            for (int i = 0; i < itemCount; i++)
            {
                var item = GenerateRandomItem();
                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items;
        }
    }
}
