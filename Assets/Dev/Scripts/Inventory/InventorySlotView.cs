using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
using TMPro;
using InventorySystem.Contracts;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    public class InventorySlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _slotBackground;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _stackCountText;
        [SerializeField] private Button _slotButton;

        private int _slotIndex;
        private IInventory _inventory;
        private InventoryConfig _config;
        private IDragService _dragService;
        private InventorySlotHighlighter _highlighter;
        private ItemTooltip _tooltip;
        private bool _isPointerOver;


        [Inject]
        public void Construct(IInventory inventory, InventoryConfig config, IDragService dragService, ItemTooltip tooltip)
        {
            _inventory = inventory;
            _config = config;
            _dragService = dragService;
            _tooltip = tooltip;
        }
        private void Awake()
        {
            _slotButton.onClick.AddListener(OnSlotClicked);
            _highlighter = GetComponent<InventorySlotHighlighter>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;

            if (_itemIcon.sprite != null)
                Debug.Log($"Навелся: {_itemIcon.sprite.name}");

            var slot = _inventory.GetSlot(_slotIndex);
            if (slot != null && !slot.IsEmpty)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                _tooltip?.ShowTooltip(_slotIndex, mousePosition);
            }
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
            _highlighter?.HideHighlight();
            CancelInvoke(nameof(ShowTooltip));
            _tooltip?.HideTooltip();
        }

        public void Initialize(int slotIndex)
        {
            _slotIndex = slotIndex;
            UpdateView();
            _highlighter.Initialize(slotIndex);
        }

        private void ShowTooltip()
        {
            if (_isPointerOver && !_dragService.IsDragging)
            {
                var slot = _inventory.GetSlot(_slotIndex);
                if (slot != null && !slot.IsEmpty)
                {
                    _tooltip?.ShowTooltip(_slotIndex, transform.position);
                }
            }
        }

        public void UpdateView()
        {
            var slot = _inventory.GetSlot(_slotIndex);

            if (slot == null)
            {
                _itemIcon.gameObject.SetActive(false);
                _itemIcon.sprite = null;
                _stackCountText.gameObject.SetActive(false);
                return;
            }

            if (slot.IsEmpty)
            {
                _stackCountText.gameObject.SetActive(false);
                _itemIcon.sprite = null;
                return;
            }

            _itemIcon.gameObject.SetActive(true);
            _itemIcon.sprite = slot.Item.Icon;
            _itemIcon.SetAllDirty();

            if (slot.Item.IsStackable && slot.ItemCount > 1)
            {
                _stackCountText.gameObject.SetActive(true);
                _stackCountText.text = slot.ItemCount.ToString();
            }
            else
            {
                _stackCountText.gameObject.SetActive(false);
            }

            Debug.Log($"Icon set to: {_itemIcon.sprite.name}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_dragService.IsDragging) return;

            _dragService.UpdateDrag(eventData.position);
            eventData.Use();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_dragService.IsDragging) return;

            _dragService.EndDrag(eventData.position);
            eventData.Use();
            UpdateView();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("MOUSE DOWN ON SLOT!");
        }

        private void OnSlotClicked()
        {
            Debug.Log($"=== SLOT CLICK DETECTED ===");
            Debug.Log($"Slot {_slotIndex} clicked");

            var slot = _inventory.GetSlot(_slotIndex);
            if (slot != null)
            {
                if (slot.IsEmpty)
                {
                    Debug.Log($"Slot {_slotIndex} clicked - Empty");
                }
                else
                {
                    Debug.Log($"Slot {_slotIndex} clicked - {slot.Item.ItemName} x{slot.ItemCount}");
                }
            }
            else
            {
                Debug.LogError($"Slot {_slotIndex} is NULL!");
            }
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"=== DRAG START DETECTED ===");
            Debug.Log($"Begin drag on slot {_slotIndex}");

            var slot = _inventory.GetSlot(_slotIndex);
            if (slot == null || slot.IsEmpty)
            {
                return;
            }

            bool isSplit = IsShiftPressed();
            Debug.Log($"Shift pressed: {isSplit}");

            if (isSplit && slot.Item.IsStackable && slot.ItemCount > 1)
            {
                _dragService.StartSplitDrag(slot, eventData.position);
            }
            else
            {
                _dragService.StartDrag(slot, eventData.position);
            }

            eventData.Use();
        }

        private bool IsShiftPressed()
        {

            return Keyboard.current.leftShiftKey.isPressed ||
                   Keyboard.current.rightShiftKey.isPressed;

        }

        private void OnDestroy()
        {
            if (_slotButton != null)
            {
                _slotButton.onClick.RemoveListener(OnSlotClicked);
            }
        }

        private void Update()
        {
            if (_isPointerOver && !_dragService.IsDragging && _tooltip != null && _tooltip.IsShowing)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                _tooltip.UpdatePosition(mousePosition);
            }

            if (_dragService != null && _dragService.IsDragging && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                _dragService.CancelDrag();
                UpdateView();
            }
        }

    }
}