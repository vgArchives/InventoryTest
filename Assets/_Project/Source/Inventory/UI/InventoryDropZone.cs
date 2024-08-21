using UnityEngine;

namespace Project.Core
{
    public class InventoryDropZone : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        
        public RectTransform RectTransform => _rectTransform;
    }
}
