using System.Collections.Generic;
using Coimbra;
using Coimbra.Services;
using UnityEngine;

namespace Project.Core
{
    [CreateAssetMenu(fileName = "EquipmentItemData", menuName = "Item/Equipment Item")]
    public class EquipmentItemData : ItemBaseData
    {
        public void AddEquipmentStats()
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

        public void RemoveEquipmentStats()
        {
            PlayerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
            SerializableDictionary<StatType, Stat> playerStats = PlayerStatsService.PlayerStats;

            foreach (KeyValuePair<StatType, int> affectedStat in AffectedStats)
            {
                if (playerStats.TryGetValue(affectedStat.Key, out Stat playerStat))
                {
                    playerStat.UpdateStat(-affectedStat.Value);
                }
            }
        }
    }
}