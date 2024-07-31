using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tatsu.Core
{
    public class InventorySlotView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _slotImage;

        private ItemBaseData _itemData;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_itemData is ConsumableItemData)
            {
                _itemData.ResolveItemBehavior();
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
        
        public void Initialize(ItemBaseData itemBaseData)
        {
            _slotImage.sprite = itemBaseData.ItemSprite;
            _slotImage.gameObject.SetActive(true);

            _itemData = itemBaseData;
        }
    }
}
