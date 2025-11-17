using InventorySystem.Contracts;
using UnityEngine;
using Zenject;

namespace InventorySystem
{
    public class InventoryDropHandler : IDropHandler
    {
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryView _inventoryView;

        public InventoryDropHandler(IInventoryService inventoryService, IInventoryView inventoryView)
        {
            _inventoryService = inventoryService;
            _inventoryView = inventoryView;
        }

        public bool CanDrop(IDragService dragService, int slotIndex)
        {
            if (!dragService.IsDragging) return false;

            var targetSlot = _inventoryService.PlayerInventory.GetSlot(slotIndex);
            var dragData = GetDragData(dragService);

            if (targetSlot == null) return false;

            return CanAcceptItem(targetSlot, dragData);
        }

        public void HandleDrop(IDragService dragService, int slotIndex)
        {
            if (!dragService.IsDragging) return;

            var targetSlot = _inventoryService.PlayerInventory.GetSlot(slotIndex);
            var sourceSlot = dragService.DraggedSlot;
            var dragData = GetDragData(dragService);

            if (targetSlot == null || sourceSlot == null) return;

            if (sourceSlot.SlotIndex == slotIndex)
            {
                return;
            }

            int sourceSlotIndex = sourceSlot.SlotIndex;
            int targetSlotIndex = slotIndex;

            if (targetSlot.IsEmpty)
            {
                HandleDropToEmptySlot(sourceSlot, targetSlot, dragData);
            }
            else if (targetSlot.Item.ItemId == dragData.Item.ItemId && targetSlot.Item.IsStackable)
            {
                HandleStackMerge(sourceSlot, targetSlot, dragData);
            }
            else
            {
                HandleSwap(sourceSlot, targetSlot, dragData);
            }

            UpdateSlotViews(sourceSlotIndex, targetSlotIndex);
        }

        private bool CanAcceptItem(IInventorySlot targetSlot, DragData dragData)
        {
            if (targetSlot.IsEmpty) return true;

            if (targetSlot.Item.CanStackWith(dragData.Item))
            {
                int availableSpace = targetSlot.Item.GetAvailableSpaceForStack();
                return availableSpace >= dragData.ItemCount;
            }

            return true;
        }

        private void HandleDropToEmptySlot(IInventorySlot sourceSlot, IInventorySlot targetSlot, DragData dragData)
        {
            if (dragData.IsSplitStack)
            {
                int remainingCount = sourceSlot.ItemCount - dragData.ItemCount;
                var splitItem = sourceSlot.Item.CloneWithStackSize(dragData.ItemCount);

                targetSlot.SetItem(splitItem, dragData.ItemCount);
                sourceSlot.SetItem(sourceSlot.Item, remainingCount);
            }
            else
            {
                targetSlot.SetItem(sourceSlot.Item, sourceSlot.ItemCount);
                sourceSlot.Clear();
            }
        }

        private void HandleStackMerge(IInventorySlot sourceSlot, IInventorySlot targetSlot, DragData dragData)
        {
            int totalCount = targetSlot.ItemCount + dragData.ItemCount;
            int maxStack = targetSlot.Item.MaxStackSize;

            if (totalCount <= maxStack)
            {
                targetSlot.IncreaseCount(dragData.ItemCount);

                if (dragData.IsSplitStack)
                {
                    sourceSlot.DecreaseCount(dragData.ItemCount);
                }
                else
                {
                    sourceSlot.Clear();
                }
            }
            else
            {
                int canAdd = maxStack - targetSlot.ItemCount;
                targetSlot.IncreaseCount(canAdd);

                if (dragData.IsSplitStack)
                {
                    sourceSlot.DecreaseCount(canAdd);
                }
                else
                {
                    sourceSlot.DecreaseCount(canAdd);
                }
            }
        }

        private void HandleSwap(IInventorySlot sourceSlot, IInventorySlot targetSlot, DragData dragData)
        {
            if (dragData.IsSplitStack)
            {
                return;
            }

            var targetItem = targetSlot.Item;
            int targetCount = targetSlot.ItemCount;

            targetSlot.SetItem(sourceSlot.Item, sourceSlot.ItemCount);

            sourceSlot.SetItem(targetItem, targetCount);
        }

        private DragData GetDragData(IDragService dragService)
        {
            return new DragData(dragService.DraggedSlot, dragService.IsSplitDrag);
        }

        private void UpdateSlotViews(params int[] slotIndices)
        {
            foreach (int slotIndex in slotIndices)
            {
                _inventoryView.UpdateSlotView(slotIndex);
            }
        }
    }
}