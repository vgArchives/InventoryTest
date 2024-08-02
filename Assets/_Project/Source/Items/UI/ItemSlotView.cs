using Coffee.UIEffects;
using Coimbra;
using Coimbra.Services;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tatsu.Core
{
    public class ItemSlotView : MonoBehaviour, 
        IPointerClickHandler,IPointerEnterHandler, IPointerExitHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _slotItemImage;
        [SerializeField] private TextMeshProUGUI _slotQuantity;
        [Space(10)]
        
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [Space(10)] 
        [SerializeField] private ItemTooltipView _itemTooltipView;
        [SerializeField] private UIDissolve _uiDissolve;
        
        private const int DefaultSortingOrder = 1;
        private const int HoverSortingOrder = 2;
        private const int DragSortingOrder = 3;

        private const float DissolveDuration = 0.25f;
        private const float DissolveEndValue = 1f;
        
        private const float DragCanvasTransparency = 0.6f;
        private const float DefaultCanvasTransparency = 1f;
        
        private Vector2 _originalPosition;
        private ItemBaseData _itemBaseData;

        private IInventoryService _inventoryService;
        private IEquipmentService _equipmentService;
        
        public ItemBaseData ItemBaseData => _itemBaseData;
        public RectTransform RectTransform => _rectTransform;

        public void OnPointerClick(PointerEventData eventData)
        {
            bool isDataNull = _itemBaseData == null;
            bool isConsumable = _itemBaseData is ConsumableItemData;
            
            if (isDataNull || !isConsumable)
            {
                return;
            }
            
            _inventoryService.ConsumeItem(this);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _canvas.sortingOrder = HoverSortingOrder;
            _itemTooltipView.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _canvas.sortingOrder = DefaultSortingOrder;
            _itemTooltipView.gameObject.SetActive(false);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = _rectTransform.anchoredPosition;
            _canvasGroup.alpha = DragCanvasTransparency;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _canvas.sortingOrder = DragSortingOrder;
            _rectTransform.anchoredPosition += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameObject dragDestinationObject = eventData.pointerEnter ;
            
            if (dragDestinationObject == null)
            {
                ResetPositionToOrigin();
                return;
            }
            
            if (dragDestinationObject.TryGetComponent(out EquipmentDropZone equipmentDropZone))
            {
                if (!CheckForSameType(_itemBaseData.ItemType, equipmentDropZone.ItemType))
                {
                    ResetPositionToOrigin();
                    return;
                }
                
                _equipmentService.EquipItem(_itemBaseData as EquipmentItemData);
                
                MoveItemToEmptySlot(equipmentDropZone.RectTransform);
                return;
            }
            
            if (dragDestinationObject.TryGetComponent(out InventoryDropZone inventoryDropZone))
            {
                MoveItemToEmptySlot(inventoryDropZone.RectTransform);
                return;
            }
            
            if (dragDestinationObject.TryGetComponent(out ItemSlotView itemSlotView))
            {
                EquipmentDropZone equipmentZone = itemSlotView.GetComponentInParent<EquipmentDropZone>();

                if (equipmentZone != null)
                {
                    if (!CheckForSameType(_itemBaseData.ItemType, itemSlotView.ItemBaseData.ItemType))
                    {
                        ResetPositionToOrigin();
                        return;
                    }
                    
                    _equipmentService.EquipItem(_itemBaseData as EquipmentItemData);
                    
                    MoveItemToOccupiedSlot(itemSlotView);
                    return;
                }
                
                InventoryDropZone inventoryZone = itemSlotView.GetComponentInParent<InventoryDropZone>();

                if (inventoryZone != null)
                {
                    MoveItemToOccupiedSlot(itemSlotView);
                    
                    return;
                }
            }
            
            ResetPositionToOrigin();
        }
        
        public void Initialize(ItemBaseData itemBaseData, int quantity)
        {
            _itemBaseData = itemBaseData;
            _slotItemImage.sprite = itemBaseData.ItemSprite;
            
            _slotQuantity.SetText($"{quantity:00}");
            _slotQuantity.gameObject.SetActive(itemBaseData.ItemType == ItemType.Consumable);
            
            _itemTooltipView.Initialize(itemBaseData.ItemName, itemBaseData.ItemDescription);
        }
        
        public void UpdateQuantity(int quantity)
        {
            if (quantity == 0)
            {
                _slotQuantity.gameObject.SetActive(false);
                RemoveItem();
                
                return;
            }
            
            _slotQuantity.SetText($"{quantity:00}");
        }
        
        protected void Start()
        {
            _inventoryService = ServiceLocator.GetChecked<IInventoryService>();
            _equipmentService = ServiceLocator.GetChecked<IEquipmentService>();
        }

        private void RemoveItem()
        {
            Sequence consumeItemSequence = DOTween.Sequence();
            consumeItemSequence.SetEase(Ease.Linear);
            consumeItemSequence.Append(DOTween.To(() => _uiDissolve.effectFactor, value => _uiDissolve.effectFactor = value,
                DissolveEndValue, DissolveDuration));
            consumeItemSequence.AppendCallback(() => gameObject.Dispose(true));
        }

        private void MoveItemToEmptySlot(RectTransform destinationParent)
        {
            _rectTransform.SetParent(destinationParent);
            _rectTransform.anchoredPosition = Vector2.zero;
                
            ResetCanvasGroup();
        }

        private void MoveItemToOccupiedSlot(ItemSlotView destinationSlot)
        {
            Transform currentSelectionParent = _rectTransform.parent;
                
            _rectTransform.SetParent(destinationSlot.RectTransform.parent);
            _rectTransform.anchoredPosition = Vector2.zero;

            destinationSlot.RectTransform.SetParent(currentSelectionParent);
            destinationSlot.RectTransform.anchoredPosition = Vector2.zero;
                
            ResetCanvasGroup();
        }

        private void ResetPositionToOrigin()
        {
            _rectTransform.anchoredPosition = _originalPosition;
            
            ResetCanvasGroup();
        }

        private void ResetCanvasGroup()
        {
            _canvasGroup.alpha = DefaultCanvasTransparency;
            _canvasGroup.blocksRaycasts = true;
            _canvas.sortingOrder = DefaultSortingOrder;
        }
        
        private bool CheckForSameType(ItemType firstItem, ItemType secondItem)
        {
            return firstItem == secondItem;
        }
    }
}
