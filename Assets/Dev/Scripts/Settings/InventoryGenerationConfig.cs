using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryGenerationConfig", menuName = "Inventory/Generation Config")]
    public class InventoryGenerationConfig : ScriptableObject
    {
        [SerializeField] private int _minItemsToGenerate = 5;
        [SerializeField] private int _maxItemsToGenerate = 10;
        [SerializeField] private int _minStackSize = 1;
        [SerializeField] private int _maxStackSize = 10;
        [SerializeField] private float _resourceSpawnChance = 0.8f;

        public int MinItemsToGenerate => _minItemsToGenerate;
        public int MaxItemsToGenerate => _maxItemsToGenerate;
        public int MinStackSize => _minStackSize;
        public int MaxStackSize => _maxStackSize;
        public float ResourceSpawnChance => _resourceSpawnChance;
    }
}