using Coimbra.Services;
using Coimbra.Services.Events;
using UnityEngine;

namespace Tatsu.Core
{
    public class PlayerVFXController : MonoBehaviour
    {
        [SerializeField] private GameObject _healthEffectPrefab;
        [SerializeField] private GameObject _manaEffectPrefab;
        
        private EventHandle _playerHealthChangeHandle;
        private EventHandle _playerManaChangeHandle;
        
        protected void Start()
        {
            _playerHealthChangeHandle = PlayerHealthChangeEvent.AddListener(HandlePlayerHealthChangeEvent);
            _playerManaChangeHandle = PlayerManaChangeEvent.AddListener(HandlePlayerManaChangeEvent);
        }
        
        protected void OnDestroy()
        {
            ServiceLocator.GetChecked<IEventService>().RemoveListener(_playerHealthChangeHandle);
            ServiceLocator.GetChecked<IEventService>().RemoveListener(_playerManaChangeHandle);
        }
        
        private void HandlePlayerHealthChangeEvent(ref EventContext context, in PlayerHealthChangeEvent e)
        {
            if (e.EffectiveHealthValue > e.PreviousEffectiveHealthValue)
            {
                Instantiate(_healthEffectPrefab, transform);
            }
        }
        
        private void HandlePlayerManaChangeEvent(ref EventContext context, in PlayerManaChangeEvent e)
        {
            if (e.EffectiveManaValue > e.PreviousEffectiveManaValue)
            {
                Instantiate(_manaEffectPrefab, transform);
            }
        }
    }
}
