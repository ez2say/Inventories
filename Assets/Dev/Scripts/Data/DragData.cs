using InventorySystem.Contracts;

namespace InventorySystem
{
    public class DragData
    {
        public IInventorySlot SourceSlot { get; }
        public IInventoryItem Item { get; }
        public int ItemCount { get; }
        public bool IsSplitStack { get; }

        public DragData(IInventorySlot sourceSlot, bool isSplitStack = false)
        {
            SourceSlot = sourceSlot;
            Item = sourceSlot.Item;
            ItemCount = isSplitStack ? sourceSlot.ItemCount / 2 : sourceSlot.ItemCount;
            IsSplitStack = isSplitStack;
        }
    }
}