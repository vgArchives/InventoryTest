using System.Collections.Generic;
using Coimbra;
using Coimbra.Services;
using UnityEngine;

namespace Tatsu.Core
{
    [CreateAssetMenu(fileName = "EquipmentItemData", menuName = "Item/Equipment Item")]
    public class EquipmentItemData : ItemBaseData
    {
        public void AddStats()
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

        public void RemoveStats()
        {
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
            SerializableDictionary<StatType, Stat> playerStats = _playerStatsService.PlayerStats;

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