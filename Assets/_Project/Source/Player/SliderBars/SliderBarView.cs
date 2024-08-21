using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Core
{
    public class SliderBarView : MonoBehaviour
    {
        [Space(5)]
        [SerializeField] private Slider _frontSlider;
        [SerializeField] private Slider _backSlider;
        [Space(5)]
        
        [SerializeField] private Image _frontSliderFillImage;
        [SerializeField] private Image _backSliderFillImage;
        [Space(5)] 
        
        [SerializeField] private TextMeshProUGUI _currentValueText;
        [SerializeField] private TextMeshProUGUI _maxValueText;
        [Space(5)]
        
        [SerializeField] private float _valueTweenDuration = 0.35f;
        [Space(10)]
        
        private int _cachedFrontValue;
        
        public void Initialize(int minValue, int maxValue, int currentValue)
        {
            _frontSlider.minValue = _backSlider.minValue = minValue;
            _frontSlider.maxValue = _backSlider.maxValue = maxValue;
            _frontSlider.value = _backSlider.value = maxValue;
            
            _currentValueText.SetText($"{currentValue:00}");
            _maxValueText.SetText($"{maxValue:00}");
        }

        public void TweenSliderValue(int newSliderValue)
        {
            float currentHealthView = _frontSlider.value;
            float currentMidHealthView = _backSlider.value;
            
            Tweener frontSliderTweener = DOTween.To(() => currentHealthView, x => currentHealthView = x, newSliderValue, _valueTweenDuration);
            frontSliderTweener.SetEase(Ease.Linear);

            frontSliderTweener.OnUpdate(() =>
            {
                _frontSlider.value = currentHealthView;
                _currentValueText.SetText($"{currentHealthView:00}");
            });

            frontSliderTweener.OnComplete(() =>
            {
                _frontSlider.value = newSliderValue;
                
                Tweener midSliderTweener = DOTween.To(() => currentMidHealthView, x => currentMidHealthView = x, newSliderValue, _valueTweenDuration);
                midSliderTweener.SetEase(Ease.OutQuad);

                midSliderTweener.OnUpdate(() =>
                {
                    _backSlider.value = currentMidHealthView;
                });

                midSliderTweener.OnComplete(() =>
                {
                    _backSlider.value = newSliderValue;
                });
            });
        }
    }
}
