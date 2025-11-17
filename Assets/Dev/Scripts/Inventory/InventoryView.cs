using InventorySystem.Contracts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem
{
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        [SerializeField] private GridLayoutGroup _slotsGrid;
        [SerializeField] private Transform _slotContainer;

        [Inject] private IInventory _inventory;
        [Inject] private DiContainer _container;

        private InventorySlotView[] _slotViews;
        private InventoryConfig _config;

        [Inject]
        public void Construct(InventoryConfig config)
        {
            _config = config;
        }

        public void Initialize(IInventory inventory)
        {
            _inventory = inventory;
            CreateSlots();
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public Transform GetSlotTransform(int slotIndex)
        {
            if (_slotViews != null && slotIndex >= 0 && slotIndex < _slotViews.Length)
                return _slotViews[slotIndex].transform;

            return null;
        }

        public void UpdateSlotView(int slotIndex)
        {
            if (_slotViews != null && slotIndex >= 0 && slotIndex < _slotViews.Length)
                _slotViews[slotIndex].UpdateView();
        }

        public void UpdateAllSlots()
        {
            if (_slotViews == null) return;

            for (int i = 0; i < _slotViews.Length; i++)
                _slotViews[i].UpdateView();
        }

        private void CreateSlots()
        {
            if (_inventory == null)
            {
                Debug.LogError("InventoryView: _inventory is null!");
                return;
            }

            if (_config == null)
            {
                Debug.LogError("InventoryView: _config is null!");
                return;
            }

            if (_config.SlotPrefab == null)
            {
                Debug.LogError("InventoryView: SlotPrefab is null!");
                return;
            }

            foreach (Transform child in _slotContainer)
                Destroy(child.gameObject);

            _slotViews = new InventorySlotView[_inventory.TotalSlots];

            for (int i = 0; i < _inventory.TotalSlots; i++)
            {
                var slotView = _container.InstantiatePrefabForComponent<InventorySlotView>(
                    _config.SlotPrefab,
                    _slotContainer
                );

                slotView.Initialize(i);

                _slotViews[i] = slotView;
            }

            SetupGridLayout();
        }

        private void SetupGridLayout()
        {
            if (_slotsGrid != null)
            {
                _slotsGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                _slotsGrid.constraintCount = _inventory.Width;
            }
        }
    }
}
