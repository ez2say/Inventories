using InventorySystem.Contracts;
using Zenject;

namespace InventorySystem
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryConfig _config;
        private readonly IItemService _itemService;
        private readonly IInventoryGenerator _inventoryGenerator;
        private Inventory _playerInventory;

        public IInventory PlayerInventory => _playerInventory;

        public InventoryService(
            InventoryConfig config,
            IItemService itemService,
            IInventoryGenerator inventoryGenerator)
        {
            _config = config;
            _itemService = itemService;
            _inventoryGenerator = inventoryGenerator;
        }

        public void Initialize()
        {
            _playerInventory = new Inventory(_config.Width, _config.Height);
            _inventoryGenerator.GenerateInitialInventory(_playerInventory);
        }

        public bool TryAddItemToSlot(int slotIndex, string itemId, int count = 1)
        {
            var slot = _playerInventory.GetSlot(slotIndex);
            if (slot == null || !slot.IsEmpty) return false;

            var item = _itemService.CreateItem(itemId, count);
            if (item == null) return false;

            slot.SetItem(item, count);
            return true;
        }

        public bool TryRemoveItemFromSlot(int slotIndex)
        {
            var slot = _playerInventory.GetSlot(slotIndex);
            if (slot == null || slot.IsEmpty) return false;

            slot.Clear();
            return true;
        }

        public void RegenerateInventory()
        {
            _inventoryGenerator.GenerateInitialInventory(_playerInventory);
        }
    }
}