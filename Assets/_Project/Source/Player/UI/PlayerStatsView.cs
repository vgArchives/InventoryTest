using System.Collections.Generic;
using Coimbra.Services;
using UnityEngine;

namespace Project.Core
{
    public class PlayerStatsView : MonoBehaviour
    {
        [SerializeField] private GameObject _playerStatSlotPrefab;
        [SerializeField] private Transform _parentTransform;
        [Space(10)]
        
        [SerializeField] private List<StatSlotView> _statSlotViews = new ();
        
        private IPlayerStatsService _playerStatsService;

        protected void Start()
        {
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
            
            InitializeStatSlots();
        }

        private void InitializeStatSlots()
        {
            int count = _statSlotViews.Count;
            int i = 0;
            
            foreach (KeyValuePair<StatType, Stat> playerStat in _playerStatsService.PlayerStats)
            {
                if (i >= count)
                {
                    break;
                }
                
                _statSlotViews[i].Initialize(playerStat);
                i++;
            }
        }
    }
}
