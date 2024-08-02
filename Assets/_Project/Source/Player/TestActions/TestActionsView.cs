using System;
using Coimbra.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Tatsu.Core
{
    public class TestActionsView : MonoBehaviour
    {
        [SerializeField] private Button _takeDamageButton;
        [SerializeField] private Button _useSpellButton;

        private IPlayerAnimationsService _playerAnimationsService;

        protected void Start()
        {
            _playerAnimationsService = ServiceLocator.GetChecked<IPlayerAnimationsService>();
            
            _takeDamageButton.onClick.AddListener(HandleTakeDamageButtonClick);
            _useSpellButton.onClick.AddListener(HandleUseSpellButtonClick);
        }
        
        protected void OnDestroy()
        {
            _takeDamageButton.onClick.RemoveListener(HandleTakeDamageButtonClick);
            _useSpellButton.onClick.RemoveListener(HandleUseSpellButtonClick);
        }

        private void HandleTakeDamageButtonClick()
        {
            _playerAnimationsService.PlayTriggerAnimation(AnimationType.TakeDamage);
        }
        
        private void HandleUseSpellButtonClick()
        {
            _playerAnimationsService.PlayTriggerAnimation(AnimationType.Attack);
        }
    }
}
