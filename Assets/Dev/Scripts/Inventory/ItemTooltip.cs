using UnityEngine;
using TMPro;
using Zenject;
using InventorySystem.Contracts;

namespace InventorySystem
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private GameObject _tooltipPanel;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemDescriptionText;
        [SerializeField] private TextMeshProUGUI _itemStatsText;

        private IInventory _inventory;
        private Canvas _canvas;
        private RectTransform _canvasRect;
        public bool IsShowing => _isShowing;
        private bool _isShowing = false;
        private int _currentSlotIndex = -1;

        [Inject]
        public void Construct(IInventory inventory, Canvas canvas)
        {
            _inventory = inventory;
            _canvas = canvas;
            _canvasRect = canvas.GetComponent<RectTransform>();

            HideTooltip();
        }

        public void ShowTooltip(int slotIndex, Vector2 screenPosition)
        {
            var slot = _inventory.GetSlot(slotIndex);
            if (slot == null || slot.IsEmpty) return;

            if (_isShowing && _currentSlotIndex == slotIndex)
            {
                UpdateTooltipPosition(screenPosition);
                return;
            }


            _itemNameText.text = slot.Item.ItemName;
            _itemDescriptionText.text = GetItemDescription(slot.Item);
            _itemStatsText.text = GetItemStats(slot.Item, slot.ItemCount);

            _tooltipPanel.SetActive(true);
            _isShowing = true;
            _currentSlotIndex = slotIndex;

            UpdateTooltipPosition(screenPosition);

            Debug.Log($"Tooltip shown for {slot.Item.ItemName}");
        }

        private void UpdateTooltipPosition(Vector2 screenPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect,
                screenPosition,
                _canvas.worldCamera,
                out Vector2 localPoint
            );


            localPoint += new Vector2(30, -20);


            RectTransform tooltipRect = _tooltipPanel.GetComponent<RectTransform>();
            Vector2 clampedPosition = GetClampedPosition(localPoint, tooltipRect, _canvasRect);

            tooltipRect.anchoredPosition = clampedPosition;
        }
        public void UpdatePosition(Vector2 screenPosition)
        {
            if (_isShowing)
            {
                UpdateTooltipPosition(screenPosition);
            }
        }

        private Vector2 GetClampedPosition(Vector2 position, RectTransform tooltipRect, RectTransform canvasRect)
        {
            Vector2 clamped = position;

            Vector2 tooltipSize = tooltipRect.rect.size;

            Vector2 canvasSize = canvasRect.rect.size;
            Vector2 canvasCenter = canvasRect.rect.center;


            float minX = -canvasSize.x / 2 + tooltipSize.x / 2;
            float maxX = canvasSize.x / 2 - tooltipSize.x / 2;
            float minY = -canvasSize.y / 2 + tooltipSize.y / 2;
            float maxY = canvasSize.y / 2 - tooltipSize.y / 2;

            clamped.x = Mathf.Clamp(clamped.x, minX, maxX);
            clamped.y = Mathf.Clamp(clamped.y, minY, maxY);

            return clamped;
        }

        public void HideTooltip()
        {
            _tooltipPanel.SetActive(false);
            _isShowing = false;
            _currentSlotIndex = -1;
        }

        private string GetItemDescription(IInventoryItem item)
        {
            return item.ItemType switch
            {
                ItemType.Resource => "Ресурс для крафта",
                ItemType.Tool => "Инструмент",
                _ => "Предмет"
            };
        }

        private string GetItemStats(IInventoryItem item, int count)
        {
            if (item.IsStackable)
            {
                return $"Количество: {count}/{item.MaxStackSize}";
            }
            else
            {
                return "Не стакается";
            }
        }
    }
}