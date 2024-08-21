using Coffee.UIEffects;
using Coimbra.Services;
using Coimbra.Services.Events;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ColorMode = Coffee.UIEffects.ColorMode;

namespace Project.Core
{
    public class PlayerFrameView : MonoBehaviour
    {
        [SerializeField] private Image _playerImage;
        [SerializeField] private TextMeshProUGUI _playerName;
        [Space(10)]
        
        [SerializeField] private SliderBarView _healthSlider;
        [SerializeField] private SliderBarView _manaSlider;
        [Space(10)]
        
        [SerializeField] private UIEffect _uiEffect;
        
        [Header("Shake Hud Parameters")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _shakeAmount = 4;
        [SerializeField] private int _vibrato = 30;
        [SerializeField] private float _randomness = 90f;

        private EventHandle _playerHealthChangeHandle;
        private EventHandle _playerManaChangeHandle;
        private IPlayerStatsService _playerStatsService;

        protected void Start()
        {
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
            
            _playerHealthChangeHandle = PlayerHealthChangeEvent.AddListener(HandlePlayerHealthChangeEvent);
            _playerManaChangeHandle = PlayerManaChangeEvent.AddListener(HandlePlayerManaChangeEvent);
            
            Initialize(_playerStatsService.PlayerData);
        }
        
        protected void OnDestroy()
        {
            ServiceLocator.GetChecked<IEventService>().RemoveListener(_playerHealthChangeHandle);
            ServiceLocator.GetChecked<IEventService>().RemoveListener(_playerManaChangeHandle);
        }
        
        private void Initialize(PlayerInformationData playerData)
        {
            _playerImage.sprite = playerData.PlayerAvatarSprite;
            _playerName.SetText(playerData.PlayerName);

            Stat playerHealth = _playerStatsService.GetStat(StatType.Health);
            Stat playerMana = _playerStatsService.GetStat(StatType.Mana);
            
            _healthSlider.Initialize(0, playerHealth.MaximumValue, playerHealth.EffectiveValue);
            _manaSlider.Initialize(0, playerMana.MaximumValue, playerMana.EffectiveValue);
        }
        
        private void HandlePlayerHealthChangeEvent(ref EventContext context, in PlayerHealthChangeEvent e)
        {
            _healthSlider.TweenSliderValue(e.EffectiveHealthValue);

            if (e.PreviousEffectiveHealthValue <= e.EffectiveHealthValue)
            {
                return;
            }
            
            TatsuDOTweenUtils.ShakeUIObject(transform, _duration, _shakeAmount, _vibrato, _randomness);
            TweenPlayerAvatarDamage();
        }
        
        private void HandlePlayerManaChangeEvent(ref EventContext context, in PlayerManaChangeEvent e)
        {
            _manaSlider.TweenSliderValue(e.EffectiveManaValue);
        }

        private void TweenPlayerAvatarDamage()
        {
            _uiEffect.effectMode = EffectMode.None;
            _uiEffect.effectFactor = 1f;
            _uiEffect.colorMode = ColorMode.Fill;
            _playerImage.color = Color.red;
            _uiEffect.colorFactor = 0f;
            
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.Linear);
            sequence.Append(DOTween.To(() => _uiEffect.colorFactor, x => _uiEffect.colorFactor = x, 0.65f, 0.2f));
            sequence.Append(DOTween.To(() => _uiEffect.colorFactor, x => _uiEffect.colorFactor = x, 0f, 0.2f));
        }
    }
}
