using UnityEngine;

namespace InventorySystem.Contracts
{
    public interface IDragService
    {
        bool IsDragging { get; }
        IInventorySlot DraggedSlot { get; }
        GameObject DragObject { get; }
        bool IsSplitDrag { get; }

        void StartDrag(IInventorySlot slot, Vector2 screenPosition);
        void StartSplitDrag(IInventorySlot slot, Vector2 screenPosition);
        void UpdateDrag(Vector2 screenPosition);
        void EndDrag(Vector2 screenPosition);
        void CancelDrag();
    }
}