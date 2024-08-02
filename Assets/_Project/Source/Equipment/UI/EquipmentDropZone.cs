using UnityEngine;

namespace Tatsu.Core
{
    public class EquipmentDropZone : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private ItemType _itemType;
        
        public RectTransform RectTransform => _rectTransform;
        public ItemType ItemType => _itemType;
    }
}