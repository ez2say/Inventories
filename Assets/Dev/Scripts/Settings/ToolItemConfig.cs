using UnityEngine;

namespace InventorySystem
{
    public enum ToolType
    {
        Axe,
        Pickaxe,
        Hammer
    }


    [CreateAssetMenu(fileName = "ToolConfig", menuName = "Inventory/Tool Config")]
    public class ToolItemConfig : ItemConfig
    {
        [SerializeField] private ToolType _toolType;

        public ToolType ToolType => _toolType;
    }
}