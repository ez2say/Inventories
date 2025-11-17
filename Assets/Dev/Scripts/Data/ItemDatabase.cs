using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] private List<ResourceItemConfig> _resourceConfigs;
        [SerializeField] private List<ToolItemConfig> _toolConfigs;

        public IReadOnlyList<ResourceItemConfig> ResourceConfigs => _resourceConfigs;
        public IReadOnlyList<ToolItemConfig> ToolConfigs => _toolConfigs;

        public ItemConfig GetItemConfigById(string itemId)
        {
            foreach (var config in _resourceConfigs)
            {
                if (config.ItemId == itemId) return config;
            }

            foreach (var config in _toolConfigs)
            {
                if (config.ItemId == itemId) return config;
            }

            return null;
        }

        public List<ItemConfig> GetAllItemConfigs()
        {
            var allConfigs = new List<ItemConfig>();
            allConfigs.AddRange(_resourceConfigs);
            allConfigs.AddRange(_toolConfigs);
            return allConfigs;
        }
    }
}