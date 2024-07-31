using System.Collections.Generic;
using Coimbra.Services;
using UnityEngine;

namespace Tatsu.Core
{
    public class PlayerStatsView : MonoBehaviour
    {
        [SerializeField] private GameObject _playerStatSlotPrefab;
        [SerializeField] private Transform _parentTransform;
        
        private IPlayerStatsService _playerStatsService;

        protected void Start()
        {
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
            
            InitializeStatSlots();
        }

        private void InitializeStatSlots()
        {
            foreach (KeyValuePair<StatType, Stat> playerStat in _playerStatsService.PlayerStats)
            {
                GameObject slotObject = Instantiate(_playerStatSlotPrefab, _parentTransform);
                StatSlotView statSlotView = slotObject.GetComponent<StatSlotView>();
                statSlotView.Initialize(playerStat);
            }
        }
    }
}
