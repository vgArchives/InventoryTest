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