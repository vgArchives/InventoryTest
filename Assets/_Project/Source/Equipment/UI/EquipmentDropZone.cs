using Coimbra.Services;
using UnityEngine;

namespace Tatsu.Core
{
    public class EquipmentDropZone : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private ItemType _itemType;
        
        private IEquipmentService _equipmentService;
        
        public RectTransform RectTransform => _rectTransform;
        public ItemType ItemType => _itemType;

        protected void Start()
        {
            _equipmentService = ServiceLocator.GetChecked<IEquipmentService>();
        }

        protected void OnTransformChildrenChanged()
        {
            if (transform.childCount == 0)
            {
                _equipmentService.UnequipItem(_itemType);
            }
        }
    }
}