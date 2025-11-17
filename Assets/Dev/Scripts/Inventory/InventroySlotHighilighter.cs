using InventorySystem.Contracts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem
{
    public class InventorySlotHighlighter : MonoBehaviour
    {
        [SerializeField] private Image _highlightImage;
        [SerializeField] private Color _canDropColor = new Color(0, 1, 0, 0.3f);
        [SerializeField] private Color _cannotDropColor = new Color(1, 0, 0, 0.3f);
        [SerializeField] private Color _neutralColor = new Color(1, 1, 1, 0.1f);

        private IDragService _dragService;
        private IDropHandler _dropHandler;
        private InventorySlotView _slotView;
        private int _slotIndex;

        [Inject]
        public void Construct(IDragService dragService, IDropHandler dropHandler)
        {
            _dragService = dragService;
            _dropHandler = dropHandler;
        }

        private void Awake()
        {
            _slotView = GetComponent<InventorySlotView>();
            _highlightImage.gameObject.SetActive(false);
        }

        public void Initialize(int slotIndex)
        {
            _slotIndex = slotIndex;
        }

        private void Update()
        {
            UpdateHighlight();
        }

        private void UpdateHighlight()
        {
            bool canDrop = _dropHandler.CanDrop(_dragService, _slotIndex);

            _highlightImage.gameObject.SetActive(true);
            _highlightImage.color = canDrop ? _canDropColor : _neutralColor;
        }


        public void ShowNeutralHighlight()
        {
            _highlightImage.gameObject.SetActive(true);
            _highlightImage.color = _neutralColor;
        }

        public void HideHighlight()
        {
            _highlightImage.gameObject.SetActive(false);
        }
    }
}