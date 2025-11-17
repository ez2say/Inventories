using InventorySystem.Contracts;
using UnityEngine;
using Zenject;

namespace InventorySystem
{
    public class InventoryStarter : MonoBehaviour
    {
        private IInventoryService _inventoryService;
        private IInventoryView _inventoryView;
        private IItemGeneratorService _itemGenerator;

        [Inject]
        public void Construct(
            IInventoryService inventoryService,
            IInventoryView inventoryView,
            IItemGeneratorService itemGenerator)
        {
            _inventoryService = inventoryService;
            _inventoryView = inventoryView;
            _itemGenerator = itemGenerator;

            InitializeInventory();
        }

        private void InitializeInventory()
        {

            _inventoryView.Initialize(_inventoryService.PlayerInventory);
            _inventoryView.Show();

            if (_inventoryView is InventoryView concreteView)
            {
                concreteView.UpdateAllSlots();
                foreach (Transform slotTransform in concreteView.GetComponentsInChildren<Transform>())
                {
                    var slotView = slotTransform.GetComponent<InventorySlotView>();
                    if (slotView != null)
                    {
                        slotView.UpdateView();
                    }
                }
            }

            LogInventoryContents();
        }

        private void LogInventoryContents()
        {
            Debug.Log("=== Initial Inventory Contents ===");
            int emptySlots = 0;
            int filledSlots = 0;

            for (int i = 0; i < _inventoryService.PlayerInventory.TotalSlots; i++)
            {
                var slot = _inventoryService.PlayerInventory.GetSlot(i);
                if (slot.IsEmpty)
                {
                    emptySlots++;
                }
                else
                {
                    filledSlots++;
                    Debug.Log($"Slot {i}: {slot.Item.ItemName} x{slot.ItemCount}");
                }
            }

            Debug.Log($"Filled slots: {filledSlots}, Empty slots: {emptySlots}");
            Debug.Log("=================================");
        }


        [ContextMenu("Regenerate Inventory")]
        private void RegenerateInventory()
        {
            for (int i = 0; i < _inventoryService.PlayerInventory.TotalSlots; i++)
            {
                _inventoryService.PlayerInventory.ClearSlot(i);
            }

            var initialItems = _itemGenerator.GenerateInitialInventory(_inventoryService.PlayerInventory.TotalSlots / 2);

            foreach (var item in initialItems)
            {
                _inventoryService.PlayerInventory.TryAddItem(item, item.CurrentStackSize);
            }

            _inventoryView.UpdateAllSlots();
            LogInventoryContents();
        }
    }
}