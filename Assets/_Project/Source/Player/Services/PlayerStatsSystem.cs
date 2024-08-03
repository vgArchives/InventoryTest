using System.Collections.Generic;
using Coimbra;
using Coimbra.Services;
using UnityEngine;

namespace Tatsu.Core
{
    public class PlayerStatsSystem : Actor, IPlayerStatsService
    {
        [Space(10)] 
        [SerializeField] private PlayerInformationData _playerData;
        [Space(10)]
        
        [SerializeField] private SerializableDictionary<StatType, Stat> _playerStats;
        
        private PlayerStatsSystem() { }

        private bool _isAlive;
        private IPlayerAnimationsService _playerAnimationsService;

        public bool IsAlive => _isAlive;
        public PlayerInformationData PlayerData => _playerData;
        public SerializableDictionary<StatType, Stat> PlayerStats => _playerStats;
        
        public void AddStatValue(StatType statType, int value)
        {
            if (!_playerStats.TryGetValue(statType, out Stat stat))
            {
                return;
            }
            
            stat.UpdateStat(value);
        }

        public void SubtractStatValue(StatType statType, int value)
        {
            if (!_isAlive)
            {
                return;
            }
            
            if (!_playerStats.TryGetValue(statType, out Stat stat))
            {
                return;
            }
            
            stat.UpdateStat(-value);
        }
        
        public Stat GetStat(StatType statType)
        {
            return _playerStats.GetValueOrDefault(statType);
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            _playerAnimationsService = ServiceLocator.GetChecked<IPlayerAnimationsService>();

            foreach (KeyValuePair<StatType, Stat> stat in _playerStats)
            {
                stat.Value.Initialize();
                
                switch (stat.Key)
                {
                    case StatType.Health:
                        stat.Value.OnStatValueChange += HandleHealthValueChange;
                        break;
                    case StatType.Mana:
                        stat.Value.OnStatValueChange += HandleManaValueChange;
                        break;
                }
            }

            _isAlive = true;
        }

        private void HandleHealthValueChange(int originalValue, int effectiveValue, int maximumValue, int previousEffectiveValue)
        {
            if (effectiveValue == 0)
            {  
                _playerAnimationsService.SetBoolParameter(AnimationType.Fainted, true);
            }
            
            SetAliveStatus(effectiveValue);

            new PlayerHealthChangeEvent(effectiveValue, previousEffectiveValue).Invoke(this);
        }
        
        private void HandleManaValueChange(int originalValue, int effectiveValue, int maximumValue, int previousEffectiveValue)
        {
            new PlayerManaChangeEvent(effectiveValue, previousEffectiveValue).Invoke(this);
        }

        private void SetAliveStatus(int healtValue)
        {
            _isAlive = healtValue > 0;
        }
    }
}