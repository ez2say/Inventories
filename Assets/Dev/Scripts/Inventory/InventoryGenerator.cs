using InventorySystem.Contracts;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace InventorySystem
{
    public class InventoryGenerator : IInventoryGenerator
    {
        private readonly IItemService _itemService;
        private readonly InventoryGenerationConfig _generationConfig;
        private readonly System.Random _random;

        public InventoryGenerator(IItemService itemService, InventoryGenerationConfig generationConfig)
        {
            _itemService = itemService;
            _generationConfig = generationConfig;
            _random = new System.Random();
        }

        public void GenerateInitialInventory(IInventory inventory)
        {
            if (inventory == null) return;

            ClearInventory(inventory);
            FillRandomItems(inventory);
        }

        private void ClearInventory(IInventory inventory)
        {
            for (int i = 0; i < inventory.TotalSlots; i++)
            {
                var slot = inventory.GetSlot(i);
                slot?.Clear();
            }
        }

        private void FillRandomItems(IInventory inventory)
        {
            int itemsToGenerate = _random.Next(
                _generationConfig.MinItemsToGenerate,
                _generationConfig.MaxItemsToGenerate + 1
            );

            var availableSlots = GetAvailableSlots(inventory);
            ShuffleSlots(availableSlots);

            for (int i = 0; i < Mathf.Min(itemsToGenerate, availableSlots.Count); i++)
            {
                var slot = availableSlots[i];
                var itemConfig = GetRandomItemConfig();

                if (itemConfig != null)
                {
                    int stackSize = GetRandomStackSize(itemConfig);
                    var item = _itemService.CreateItem(itemConfig.ItemId, stackSize);

                    if (item != null)
                    {
                        slot.SetItem(item, stackSize);
                    }
                }
            }
        }

        private List<IInventorySlot> GetAvailableSlots(IInventory inventory)
        {
            var slots = new List<IInventorySlot>();
            for (int i = 0; i < inventory.TotalSlots; i++)
            {
                var slot = inventory.GetSlot(i);
                if (slot != null && slot.IsEmpty)
                {
                    slots.Add(slot);
                }
            }
            return slots;
        }

        private void ShuffleSlots(List<IInventorySlot> slots)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                int randomIndex = _random.Next(i, slots.Count);
                var temp = slots[i];
                slots[i] = slots[randomIndex];
                slots[randomIndex] = temp;
            }
        }

        private ItemConfig GetRandomItemConfig()
        {
            var itemDatabase = _itemService.ItemDatabase;

            if (_random.NextDouble() < _generationConfig.ResourceSpawnChance)
            {
                if (itemDatabase.ResourceConfigs.Count > 0)
                {
                    int index = _random.Next(0, itemDatabase.ResourceConfigs.Count);
                    return itemDatabase.ResourceConfigs[index];
                }
            }
            else
            {
                if (itemDatabase.ToolConfigs.Count > 0)
                {
                    int index = _random.Next(0, itemDatabase.ToolConfigs.Count);
                    return itemDatabase.ToolConfigs[index];
                }
            }

            var allConfigs = itemDatabase.GetAllItemConfigs();
            if (allConfigs.Count > 0)
            {
                int index = _random.Next(0, allConfigs.Count);
                return allConfigs[index];
            }

            return null;
        }

        private int GetRandomStackSize(ItemConfig itemConfig)
        {
            if (!itemConfig.IsStackable) return 1;

            return _random.Next(
                _generationConfig.MinStackSize,
                Mathf.Min(_generationConfig.MaxStackSize, itemConfig.MaxStackSize) + 1
            );
        }
    }
}