using System.Collections.Generic;
using Coimbra;
using Coimbra.Services;
using UnityEngine;

namespace Tatsu.Core
{
    [CreateAssetMenu(fileName = "HealBehaviorData", menuName = "Item Behavior/Heal")]
    public class HealBehaviorData : ItemBehaviorBaseData
    {
        private IPlayerStatsService _playerStatsService;
        
        public override void ResolveItemBehavior(SerializableDictionary<StatType, int> affectedStats)
        {
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
            SerializableDictionary<StatType, Stat> playerStats = _playerStatsService.PlayerStats;

            foreach (KeyValuePair<StatType, int> affectedStat in affectedStats)
            {
                foreach (KeyValuePair<StatType, Stat> playerStat in playerStats)
                {
                    if (playerStat.Key == affectedStat.Key)
                    {
                        playerStat.Value.UpdateStatValue(affectedStat.Value);
                    }
                }
            }
        }
    }
}