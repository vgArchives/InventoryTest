using System.Collections.Generic;
using Coimbra;
using Coimbra.Services;
using UnityEngine;

namespace Tatsu.Core
{
    public class PlayerStatsSystem : Actor, IPlayerStatsService
    {
        [Space(10)]
        [SerializeField] private SerializableDictionary<StatType, Stat> _playerStats;
        
        private PlayerStatsSystem() { }

        private bool _isAlive;
        private IPlayerAnimationsService _playerAnimationsService;

        public bool IsAlive => _isAlive;
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

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _playerAnimationsService = ServiceLocator.GetChecked<IPlayerAnimationsService>();

            foreach (KeyValuePair<StatType, Stat> stat in _playerStats)
            {
                stat.Value.Initialize();
                
                if (stat.Key== StatType.Health)
                {
                    stat.Value.OnStatValueChange += HandleHealthValueChange;
                }
            }

            _isAlive = true;
        }

        private void HandleHealthValueChange(int originalValue, int effectiveValue, int maximumValue)
        {
            if (effectiveValue == 0)
            {  
                _playerAnimationsService.SetBoolParameter(AnimationType.Fainted, true);
            }
            
            SetAliveStatus(effectiveValue);
        }

        private void SetAliveStatus(int healtValue)
        {
            _isAlive = healtValue > 0;
        }
    }
}