using System.Collections.Generic;
using Coimbra;
using Coimbra.Services;
using UnityEngine;

namespace Project.Core
{
    [CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Item/Consumable Item")]
    public class ConsumableItemData : ItemBaseData
    {
        public void ConsumeItem()
        {
            PlayerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
            SerializableDictionary<StatType, Stat> playerStats = PlayerStatsService.PlayerStats;

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