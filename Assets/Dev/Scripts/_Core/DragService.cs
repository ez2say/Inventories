using InventorySystem.Contracts;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class DragService : IDragService
    {
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryView _inventoryView;
        private readonly Contracts.IDropHandler _dropHandler;
        private readonly Canvas _canvas;

        private DragData _currentDragData;
        private GameObject _dragObject;

        public bool IsDragging => _currentDragData != null;
        public bool IsSplitDrag { get; private set; }
        public IInventorySlot DraggedSlot => _currentDragData?.SourceSlot;
        public GameObject DragObject => _dragObject;

        public DragService(
            IInventoryService inventoryService,
            IInventoryView inventoryView,
            Contracts.IDropHandler dropHandler,
            Canvas canvas)
        {
            _inventoryService = inventoryService;
            _inventoryView = inventoryView;
            _dropHandler = dropHandler;
            _canvas = canvas;
        }

        public void StartDrag(IInventorySlot slot, Vector2 screenPosition)
        {
            if (slot == null || slot.IsEmpty)
            {
                return;
            }

            _currentDragData = new DragData(slot,false);
            IsSplitDrag = false;
            CreateDragVisual(slot, screenPosition);
        }

        public void StartSplitDrag(IInventorySlot slot, Vector2 screenPosition)
        {
            if (slot == null || slot.IsEmpty || !slot.Item.IsStackable || slot.ItemCount <= 1) return;

            _currentDragData = new DragData(slot, true);
            IsSplitDrag =  true;
            CreateDragVisual(slot, screenPosition);
        }

        public void UpdateDrag(Vector2 screenPosition)
        {
            if (!IsDragging || _dragObject == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                screenPosition,
                _canvas.worldCamera,
                out Vector2 localPoint
            );

            _dragObject.transform.localPosition = localPoint;
        }

        public void EndDrag(Vector2 screenPosition)
        {
            if (!IsDragging) return;
            HandleDrop(screenPosition);
            CleanupDrag();
        }

        public void CancelDrag()
        {
            if (!IsDragging) return;
            CleanupDrag();
        }

        private void CreateDragVisual(IInventorySlot slot, Vector2 screenPosition)
        {
            var slotTransform = _inventoryView.GetSlotTransform(slot.SlotIndex);
            if (slotTransform == null) return;

            _dragObject = new GameObject("DragVisual");
            var rectTransform = _dragObject.AddComponent<RectTransform>();
            var image = _dragObject.AddComponent<UnityEngine.UI.Image>();
            var canvasGroup = _dragObject.AddComponent<CanvasGroup>();


            image.sprite = slot.Item.Icon;
            image.raycastTarget = false;
            canvasGroup.alpha = 0.7f;
            canvasGroup.blocksRaycasts = false;

            _dragObject.transform.SetParent(_canvas.transform, false);
            rectTransform.sizeDelta = new Vector2(50, 50);

            UpdateDrag(screenPosition);
        }

        private void HandleDrop(Vector2 screenPosition)
        {
            if (!IsDragging) return;

            var targetSlotIndex = FindTargetSlotIndex(screenPosition);

            if (targetSlotIndex >= 0)
            {
                _dropHandler.HandleDrop(this, targetSlotIndex);
            }
            else
            {
                HandleDropOutsideInventory();
            }
        }
        private int FindTargetSlotIndex(Vector2 screenPosition)
        {
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (var result in results)
            {
                var slotView = result.gameObject.GetComponent<InventorySlotView>();
                if (slotView != null)
                {
                    return GetSlotIndexFromView(slotView);
                }
            }

            return -1;
        }

        private int GetSlotIndexFromView(InventorySlotView slotView)
        {
            for (int i = 0; i < _inventoryService.PlayerInventory.TotalSlots; i++)
            {
                var transform = _inventoryView.GetSlotTransform(i);
                if (transform != null && transform.gameObject == slotView.gameObject)
                {
                    return i;
                }
            }
            return -1;
        }

        private void HandleDropOutsideInventory()
        {
            if (_currentDragData == null) return;

            var sourceSlot = _currentDragData.SourceSlot;

            if (_currentDragData.IsSplitStack)
            {
                sourceSlot.DecreaseCount(_currentDragData.ItemCount);
            }
            else
            {
                sourceSlot.Clear();
            }

            _inventoryView.UpdateSlotView(sourceSlot.SlotIndex);
        }

        private void CleanupDrag()
        {
            if (_dragObject != null)
            {
                Object.Destroy(_dragObject);
                _dragObject = null;
            }
            _currentDragData = null;
        }
    }
}