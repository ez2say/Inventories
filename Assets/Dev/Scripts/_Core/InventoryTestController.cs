using InventorySystem.Contracts;
using UnityEngine;
using Zenject;

namespace InventorySystem
{
    public class InventoryDemoController : MonoBehaviour
    {
        [Header("Debug Controls")]
        [SerializeField] private KeyCode _toggleInventoryKey = KeyCode.I;
        [SerializeField] private KeyCode _regenerateKey = KeyCode.R;
        [SerializeField] private KeyCode _clearKey = KeyCode.C;

        private IInventoryService _inventoryService;
        private IInventoryView _inventoryView;

        [Inject]
        public void Construct(
            IInventoryService inventoryService,
            IInventoryView inventoryView)
        {
            _inventoryService = inventoryService;
            _inventoryView = inventoryView;
        }

        private void Start()
        {
            Debug.Log("Controls:");
            Debug.Log("- I: Toggle Inventory");
            Debug.Log("- R: Regenerate Inventory");
            Debug.Log("- C: Clear Inventory");
            Debug.Log("- Shift + Drag: Split Stack");
            Debug.Log("- Drag outside: Delete Item");
        }

        private void Update()
        {
            HandleDebugInput();
        }

        private void HandleDebugInput()
        {
            bool togglePressed = GetKeyDown(_toggleInventoryKey);
            bool regeneratePressed = GetKeyDown(_regenerateKey);
            bool clearPressed = GetKeyDown(_clearKey);

            if (togglePressed)
            {
                ToggleInventory();
            }

            if (regeneratePressed)
            {
                RegenerateInventory();
            }

            if (clearPressed)
            {
                ClearInventory();
            }
        }

        private bool GetKeyDown(KeyCode key)
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(key);
#else
            return UnityEngine.InputSystem.Keyboard.current[KeyCodeToInputSystemKey(key)].wasPressedThisFrame;
#endif
        }

#if !ENABLE_LEGACY_INPUT_MANAGER
        private UnityEngine.InputSystem.Key KeyCodeToInputSystemKey(KeyCode keyCode)
        {
            return keyCode switch
            {
                KeyCode.I => UnityEngine.InputSystem.Key.I,
                KeyCode.R => UnityEngine.InputSystem.Key.R,
                KeyCode.C => UnityEngine.InputSystem.Key.C,
                _ => UnityEngine.InputSystem.Key.None
            };
        }
#endif

        private void ToggleInventory()
        {
            if (_inventoryView is MonoBehaviour view)
            {
                view.gameObject.SetActive(!view.gameObject.activeInHierarchy);
                Debug.Log($"Inventory {(view.gameObject.activeInHierarchy ? "shown" : "hidden")}");
            }
        }

        private void RegenerateInventory()
        {
            _inventoryService.RegenerateInventory();
            UpdateInventoryView();
            Debug.Log("Inventory regenerated!");
        }

        private void ClearInventory()
        {
            var inventory = _inventoryService.PlayerInventory;
            for (int i = 0; i < inventory.TotalSlots; i++)
            {
                var slot = inventory.GetSlot(i);
                slot?.Clear();
            }
            UpdateInventoryView();
            Debug.Log("Inventory cleared!");
        }

        private void UpdateInventoryView()
        {
            if (_inventoryView is InventoryView concreteView)
            {
                concreteView.UpdateAllSlots();
            }
        }

        [ContextMenu("Log Inventory State")]
        private void LogInventoryState()
        {
            var inventory = _inventoryService.PlayerInventory;
            int filledSlots = 0;
            int totalItems = 0;

            Debug.Log("=== INVENTORY STATE ===");
            for (int i = 0; i < inventory.TotalSlots; i++)
            {
                var slot = inventory.GetSlot(i);
                if (!slot.IsEmpty)
                {
                    filledSlots++;
                    totalItems += slot.ItemCount;
                    Debug.Log($"Slot {i}: {slot.Item.ItemName} x{slot.ItemCount}");
                }
            }

            Debug.Log($"=== SUMMARY: {filledSlots}/{inventory.TotalSlots} slots filled, {totalItems} total items ===");
        }

        [ContextMenu("Add Test Item to Slot 0")]
        private void AddTestItem()
        {
            _inventoryService.TryAddItemToSlot(0, "wood", 5);
            UpdateInventoryView();
            Debug.Log("Test item added to slot 0");
        }
    }
}