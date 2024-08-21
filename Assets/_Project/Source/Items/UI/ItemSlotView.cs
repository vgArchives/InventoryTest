using Coffee.UIEffects;
using Coimbra;
using Coimbra.Services;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Core
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
        
        [SerializeField] private SlotType _currentSlotType;
        
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
        
        private const int PoisonItemId = 3;
        
        private Vector2 _originalPosition;
        private ItemBaseData _itemBaseData;

        private IInventoryService _inventoryService;
        private IEquipmentService _equipmentService;
        private IPlayerAnimationsService _playerAnimationsService;
        private IPlayerStatsService _playerStatsService;
        
        public ItemBaseData ItemBaseData => _itemBaseData;
        public RectTransform RectTransform => _rectTransform;
        public SlotType CurrentSlotType { get => _currentSlotType; set => _currentSlotType = value; }

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

            if (_itemBaseData.ItemId == PoisonItemId)
            {
                _playerAnimationsService.PlayTriggerAnimation(AnimationType.Negative);
            }
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

            new InventoryDragStartedEvent(_itemBaseData.ItemType).Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _canvas.sortingOrder = DragSortingOrder;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, 
                eventData.pressEventCamera, out Vector2 localPoint);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position - eventData.delta, 
                eventData.pressEventCamera, out Vector2 localPointDelta);
            
            _rectTransform.anchoredPosition += localPoint - localPointDelta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            new InventoryDragStoppedEvent().Invoke(this);
            
            GameObject dragDestinationObject = eventData.pointerEnter ;
            
            if (dragDestinationObject == null)
            {
                ResetItemPosition();
                return;
            }
            
            // When releasing the item on top of an empty equipment slot
            if (dragDestinationObject.TryGetComponent(out EquipmentDropZone equipmentDropZone))
            {
                bool isPlayerAlive = _playerStatsService.IsAlive;
                bool isSameType = CheckForSameType(_itemBaseData.ItemType, equipmentDropZone.ItemType);

                if (!isPlayerAlive || !isSameType)
                {
                    ResetItemPosition();
                    return;
                }

                _currentSlotType = SlotType.Equipment;
                _inventoryService.RemoveItem(_itemBaseData);
                _equipmentService.EquipItem(_itemBaseData as EquipmentItemData);

                MoveItemToEmptySlot(equipmentDropZone.RectTransform);
                return;
            }
            
            // When releasing the item on top of an empty inventory slot
            if (dragDestinationObject.TryGetComponent(out InventoryDropZone inventoryDropZone))
            {
                if (_currentSlotType == SlotType.Equipment)
                {
                    _equipmentService.UnequipItem(_itemBaseData.ItemType);
                }
                
                _currentSlotType = SlotType.Inventory;
                _inventoryService.TryAddItem(_itemBaseData);
                
                MoveItemToEmptySlot(inventoryDropZone.RectTransform);
                return;
            }
            
            if (dragDestinationObject.TryGetComponent(out ItemSlotView itemSlotView))
            {
                // When releasing the item on top of an occupied equipment slot
                EquipmentDropZone equipmentZone = itemSlotView.GetComponentInParent<EquipmentDropZone>();

                if (equipmentZone != null)
                {
                    bool isPlayerAlive = _playerStatsService.IsAlive;
                    bool isSameType = CheckForSameType(_itemBaseData.ItemType, itemSlotView.ItemBaseData.ItemType);

                    if (!isPlayerAlive || !isSameType)
                    {
                        ResetItemPosition();
                        return;
                    }
                    
                    SwapItemState(itemSlotView, SlotType.Equipment, SlotType.Inventory, _itemBaseData, 
                        itemSlotView.ItemBaseData, itemSlotView.ItemBaseData.ItemType, _itemBaseData as EquipmentItemData);
                    
                    MoveItemToOccupiedSlot(itemSlotView);
                    return;
                }
                
                // When releasing the item on top of an occupied inventory slot
                InventoryDropZone inventoryZone = itemSlotView.GetComponentInParent<InventoryDropZone>();

                if (inventoryZone != null)
                {
                    Debug.Log(_currentSlotType);
                    
                    switch (_currentSlotType)
                    {
                        case SlotType.Inventory:
                        {
                            MoveItemToOccupiedSlot(itemSlotView);
                            return;
                        }
                        case SlotType.Equipment:
                        {
                            bool isPlayerAlive = _playerStatsService.IsAlive;
                            bool isSameType = CheckForSameType(_itemBaseData.ItemType, itemSlotView.ItemBaseData.ItemType);
                            
                            if (isPlayerAlive && isSameType)
                            {
                                SwapItemState(itemSlotView, SlotType.Inventory, SlotType.Equipment, itemSlotView.ItemBaseData, 
                                    _itemBaseData, _itemBaseData.ItemType, itemSlotView.ItemBaseData as EquipmentItemData);
                                
                                MoveItemToOccupiedSlot(itemSlotView);
                            }
                            else
                            {
                                ResetItemPosition();  
                            }
                            
                            return;
                        }
                        case SlotType.None:
                        default:
                        {
                            ResetItemPosition();  
                            return;    
                        }
                    }
                }
            }
            
            ResetItemPosition();
        }

        /// <summary>
        /// This method will swap the position of two slots that are occupied by items while adding, removing, and equipping
        /// then to corresponding systems
        /// </summary>
        private void SwapItemState(ItemSlotView destinationslot, SlotType draggedNewSlotType, SlotType destinationNewSlotType, ItemBaseData removedFromInventory, 
            ItemBaseData addedToInventory, ItemType unequippedItem, EquipmentItemData equippedItem)
        {
            _currentSlotType = draggedNewSlotType;
            destinationslot.CurrentSlotType = destinationNewSlotType;
                            
            _inventoryService.RemoveItem(removedFromInventory);
            _inventoryService.TryAddItem(addedToInventory);
                            
            _equipmentService.UnequipItem(unequippedItem);
            _equipmentService.EquipItem(equippedItem);
        }
        
        public void Initialize(ItemBaseData itemBaseData, int quantity)
        {
            _itemBaseData = itemBaseData;
            _slotItemImage.sprite = itemBaseData.ItemSprite;
            
            _slotQuantity.SetText($"{quantity:00}");
            _slotQuantity.gameObject.SetActive(itemBaseData.ItemType == ItemType.Consumable);

            _currentSlotType = SlotType.Inventory;
            
            _itemTooltipView.Initialize(itemBaseData.ItemName, itemBaseData.ItemDescription);
        }
        
        public void UpdateItemQuantity(int quantity)
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
            _playerAnimationsService = ServiceLocator.GetChecked<IPlayerAnimationsService>();
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
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

        private void ResetItemPosition()
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
