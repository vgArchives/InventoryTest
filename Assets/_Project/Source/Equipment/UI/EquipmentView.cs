using Coimbra;
using Coimbra.Services;
using Coimbra.Services.Events;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Core
{
    public class EquipmentView : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ItemType, Image> _equipmentHighlights = new ();
        [Space(10)]
        
        [SerializeField] private float _blinkTweenDuration = 0.25f;

        private Image _currentHighlight;
        
        private Sequence _highlightSequence;
        private Sequence _attachedSequence;
        
        private EventHandle _inventoryDragStartedHandle;
        private EventHandle _inventoryDragStoppedHandle;

        protected void Start()
        {
            _inventoryDragStartedHandle = InventoryDragStartedEvent.AddListener(HandleInventoryDragStartedEvent);
            _inventoryDragStoppedHandle = InventoryDragStoppedEvent.AddListener(HandleInventoryDragStoppedEvent);
        }

        protected void OnDestroy()
        {
            ServiceLocator.GetChecked<IEventService>().RemoveListener(_inventoryDragStartedHandle);
            ServiceLocator.GetChecked<IEventService>().RemoveListener(_inventoryDragStoppedHandle);
        }

        private void HandleInventoryDragStartedEvent(ref EventContext context, in InventoryDragStartedEvent e)
        {
            if (!_equipmentHighlights.TryGetValue(e.ItemType, out Image highlightImage))
            {
                return;
            }
            
            _currentHighlight = highlightImage;
            TweenSlotHighlight(highlightImage);
        }
        
        private void HandleInventoryDragStoppedEvent(ref EventContext context, in InventoryDragStoppedEvent e)
        {
            StopHighlightTween();
        }

        private void TweenSlotHighlight(Image highLightImage)
        {
            highLightImage.gameObject.SetActive(true);
            
            _highlightSequence = DOTween.Sequence();
            _highlightSequence.Append(highLightImage.DOFade(1f, _blinkTweenDuration));
            _highlightSequence.Append(highLightImage.DOFade(0f, _blinkTweenDuration));
            _highlightSequence.SetEase(Ease.Linear);
            _highlightSequence.SetLoops(-1);
        }
        
        private void StopHighlightTween()
        {
            if (!IsValidSequence(_highlightSequence))
            {
                return;
            }
            
            _highlightSequence.Kill(true);
            _currentHighlight.gameObject.SetActive(false);
        }
        
        private bool IsValidSequence(Sequence sequence)
        {
            return sequence.IsActive() && sequence.IsPlaying();
        }
    }
}
