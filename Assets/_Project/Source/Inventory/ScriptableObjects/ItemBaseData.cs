using System.Collections.Generic;
using Coimbra;
using Coimbra.Services;
using UnityEngine;

namespace Tatsu.Core
{
    public abstract class ItemBaseData : ScriptableObject
    {
        [SerializeField] private string _itemName;
        
        [TextArea]
        [SerializeField] private string _itemDescription;

        [SerializeField] private Sprite _itemSprite;

        [SerializeField] private SerializableDictionary<StatType, int> _affectedStats = new ();
        
        public Sprite ItemSprite => _itemSprite;
        
        private IPlayerStatsService _playerStatsService;
        
        public void ResolveItemBehavior()
        {
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
           SerializableDictionary<StatType, Stat> playerStats = _playerStatsService.PlayerStats;

           foreach (KeyValuePair<StatType, int> affectedStat in _affectedStats)
           {
               if (playerStats.TryGetValue(affectedStat.Key, out Stat playerStat))
               {
                   playerStat.UpdateStatValue(affectedStat.Value);
               }
           }
        }
    }
}
