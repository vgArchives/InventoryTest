using Coimbra.Services;
using Coimbra.Services.Events;
using Kaardik.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tatsu.Core
{
    public class PlayerFrameView : MonoBehaviour
    {
        [SerializeField] private Image _playerImage;
        [SerializeField] private TextMeshProUGUI _playerName;
        [Space(10)]
        
        [SerializeField] private SliderBarView _healthSlider;
        [SerializeField] private SliderBarView _manaSlider;
        [Space(10)]
        
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

            if (e.PreviousEffectiveHealthValue > e.EffectiveHealthValue)
            {
                TatsuDOTweenUtils.ShakeUIObject(transform, _duration, _shakeAmount, _vibrato, _randomness);
            }
        }
        
        private void HandlePlayerManaChangeEvent(ref EventContext context, in PlayerManaChangeEvent e)
        {
            _manaSlider.TweenSliderValue(e.EffectiveHealthValue);
        }
    }
}
