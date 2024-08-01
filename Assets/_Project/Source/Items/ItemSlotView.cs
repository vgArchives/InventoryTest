using Coffee.UIEffects;
using Coimbra;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tatsu.Core
{
    public class ItemSlotView : MonoBehaviour, 
        IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _slotItemImage;
        [SerializeField] private UIDissolve _uiDissolve;
        [Space(10)]
        
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;

        private const int DefaultSortingOrder = 1;
        private const int DragSortingOrder = 2;

        private const float DissolveDuration = 0.25f;
        private const float DissolveEndValue = 1f;
        
        private const float DragCanvasTransparency = 0.6f;
        private const float DefaultCanvasTransparency = 1f;
        
        private Vector2 _originalPosition;
        private ItemBaseData _itemBaseData;
        
        private ItemBaseData ItemBaseData => _itemBaseData;
        private RectTransform RectTransform => _rectTransform;

        public void OnPointerClick(PointerEventData eventData)
        {
            bool isDataNull = _itemBaseData == null;
            bool isConsumable = _itemBaseData.ItemType == ItemType.Consumable;
            
            if (isDataNull || !isConsumable)
            {
                return;
            }
            
            //refactor
            _itemBaseData.ResolveItemBehavior();
            
            Sequence consumeItemSequence = DOTween.Sequence();
            consumeItemSequence.SetEase(Ease.Linear);
            consumeItemSequence.Append(DOTween.To(() => _uiDissolve.effectFactor, value => _uiDissolve.effectFactor = value,
                DissolveEndValue, DissolveDuration));
            consumeItemSequence.AppendCallback(() => gameObject.Dispose(true));
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = _rectTransform.anchoredPosition;

            _canvas.sortingOrder = DragSortingOrder;
            
            _canvasGroup.alpha = DragCanvasTransparency;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerEnter == null)
            {
                ResetPositionToOrigin();
                return;
            }
            
            if (eventData.pointerEnter.TryGetComponent(out EquipmentDropZone equipmentDropZone))
            {
                if (!CheckForSameType(_itemBaseData.ItemType, equipmentDropZone.ItemType))
                {
                    ResetPositionToOrigin();
                    return;
                }
                
                MoveItemToEmptySlot(equipmentDropZone.RectTransform);

                new ItemEquippedEvent(_itemBaseData as EquipmentItemData).Invoke(this);
                
                return;
            }
            
            if (eventData.pointerEnter.TryGetComponent(out InventoryDropZone inventoryDropZone))
            {
                MoveItemToEmptySlot(inventoryDropZone.RectTransform);
                return;
            }
            
            if (eventData.pointerEnter.TryGetComponent(out ItemSlotView itemSlotView))
            {
                EquipmentDropZone equipmentZone = itemSlotView.GetComponentInParent<EquipmentDropZone>();

                if (equipmentZone != null)
                {
                    if (!CheckForSameType(_itemBaseData.ItemType, itemSlotView.ItemBaseData.ItemType))
                    {
                        ResetPositionToOrigin();
                        return;
                    }
                    
                    MoveItemToOccupiedSlot(itemSlotView);
                    
                    new ItemEquippedEvent(_itemBaseData as EquipmentItemData).Invoke(this);
                    
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
        
        public void Initialize(ItemBaseData itemBaseData)
        {
            _itemBaseData = itemBaseData;
            _slotItemImage.sprite = itemBaseData.ItemSprite;
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
