using UnityEngine;

namespace InventorySystem.Contracts
{
    public interface IInventoryView
    {
        void Initialize(IInventory inventory);
        void Show();
        void Hide();
        Transform GetSlotTransform(int slotIndex);
        void UpdateSlotView(int slotIndex);
        void UpdateAllSlots();
    }
}