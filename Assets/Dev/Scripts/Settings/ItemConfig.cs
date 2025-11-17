using UnityEngine;

namespace InventorySystem
{
    public enum ItemType
    {
        Resource,
        Tool
    }


    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Inventory/Item Config")]
    public class ItemConfig : ScriptableObject
    {
        public string ItemId => _itemId;
        public string ItemName => _itemName;
        public Sprite Icon => _icon;
        public ItemType ItemType => _itemType;
        public int MaxStackSize => _maxStackSize;
        public bool IsStackable => _maxStackSize > 1;


        [SerializeField] private string _itemId;
        [SerializeField] private string _itemName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private int _maxStackSize = 1;
    }

}