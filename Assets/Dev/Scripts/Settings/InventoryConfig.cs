using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "Inventory/Inventory Config")]
    public class InventoryConfig : ScriptableObject
    {
        public int Width => _width;
        public int Height => _height;
        public GameObject SlotPrefab => _slotPrefab;
        public GameObject InventoryPanelPrefab => _inventoryPanelPrefab;
        public Sprite EmptySlotSprite => _emptySlotSprite;


        [SerializeField] private int _width = 6;
        [SerializeField] private int _height = 4;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private GameObject _inventoryPanelPrefab;
        [SerializeField] private Sprite _emptySlotSprite;

    }
}