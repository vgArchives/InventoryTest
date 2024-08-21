using Coimbra.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Core
{
    public class TestActionsView : MonoBehaviour
    {
        [SerializeField] private Button _takeDamageButton;
        [SerializeField] private Button _useSpellButton;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _quitButton;
        [Space(10)] 
        
        [SerializeField] private int _damageValue = 25;
        [SerializeField] private int _manaValue = 20;
        [SerializeField] private int _reviveHealthValue = 10;

        private IPlayerAnimationsService _playerAnimationsService;
        private IPlayerStatsService _playerStatsService;

        protected void Start()
        {
            _playerAnimationsService = ServiceLocator.GetChecked<IPlayerAnimationsService>();
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
            
            _takeDamageButton.onClick.AddListener(HandleTakeDamageButtonClick);
            _useSpellButton.onClick.AddListener(HandleUseSpellButtonClick);
            _reviveButton.onClick.AddListener(HandleReviveButtonClick);
            _quitButton.onClick.AddListener(HandleQuitButtonClick);
        }
        
        protected void OnDestroy()
        {
            _takeDamageButton.onClick.RemoveListener(HandleTakeDamageButtonClick);
            _useSpellButton.onClick.RemoveListener(HandleUseSpellButtonClick);
            _reviveButton.onClick.RemoveListener(HandleReviveButtonClick);
            _quitButton.onClick.RemoveListener(HandleQuitButtonClick);
        }
        
        private void HandleTakeDamageButtonClick()
        {
            if (!GetPlayerAliveState())
            {
                return;
            }
            
            _playerAnimationsService.PlayTriggerAnimation(AnimationType.TakeDamage);
            _playerStatsService.SubtractStatValue(StatType.Health, _damageValue);
        }
        
        private void HandleUseSpellButtonClick()
        {
            bool isPlayerAlive = GetPlayerAliveState();
            bool hasManaToCast = _playerStatsService.GetStat(StatType.Mana).EffectiveValue > 0;
            
            if (!isPlayerAlive || !hasManaToCast)
            {
                return;
            }
            
            _playerAnimationsService.PlayTriggerAnimation(AnimationType.Attack);
            _playerStatsService.SubtractStatValue(StatType.Mana, _manaValue);
        }
        
        private void HandleReviveButtonClick()
        {
            if (GetPlayerAliveState())
            {
                return;
            }
            
            _playerAnimationsService.SetBoolParameter(AnimationType.Fainted, false);
            _playerStatsService.AddStatValue(StatType.Health, _reviveHealthValue);
        }
        
        private void HandleQuitButtonClick()
        {
            Application.Quit();
        }

        private bool GetPlayerAliveState()
        {
            return _playerStatsService.IsAlive;
        }
    }
}
