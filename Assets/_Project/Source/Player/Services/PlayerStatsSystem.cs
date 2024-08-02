using System;
using System.Collections.Generic;
using Coimbra;
using UnityEngine;

namespace Tatsu.Core
{
    public class PlayerStatsSystem : Actor, IPlayerStatsService
    {
        [SerializeField] private SerializableDictionary<StatType, Stat> _playerStats;
        
        private PlayerStatsSystem() { }

        public SerializableDictionary<StatType, Stat> PlayerStats => _playerStats;
        
        public void ResetStat(StatType statType)
        {
            if (_playerStats.TryGetValue(statType, out Stat stat))
            {
                stat.ResetStat();
            }
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            foreach (KeyValuePair<StatType, Stat> stat in _playerStats)
            {
                stat.Value.Initialize();
            }
        }
    }
}