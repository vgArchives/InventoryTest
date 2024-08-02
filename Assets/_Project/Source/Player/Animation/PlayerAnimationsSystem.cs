using Coimbra;
using UnityEngine;

namespace Tatsu.Core
{
    public class PlayerAnimationsSystem : Actor, IPlayerAnimationsService
    {
        [SerializeField] private Animator _playerAnimatorController;
        private PlayerAnimationsSystem() { }

        public Animator PlayerAnimatorController => _playerAnimatorController;
        
        public void PlayTriggerAnimation(AnimationType animationType)
        {
            _playerAnimatorController.SetTrigger(animationType.ToString());
        }
    }
}
