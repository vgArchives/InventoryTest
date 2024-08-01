using System.Collections.Generic;
using Coimbra;
using Coimbra.Services;
using UnityEngine;

namespace Tatsu.Core
{
    [CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Item/Consumable Item")]
    public class ConsumableItemData : ItemBaseData
    {
        public override void ResolveItemBehavior()
        {
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
            SerializableDictionary<StatType, Stat> playerStats = _playerStatsService.PlayerStats;

            foreach (KeyValuePair<StatType, int> affectedStat in AffectedStats)
            {
                if (playerStats.TryGetValue(affectedStat.Key, out Stat playerStat))
                {
                    playerStat.UpdateStat(affectedStat.Value);
                }
            }
        }
    }
}