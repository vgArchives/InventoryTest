using Coimbra;
using UnityEngine;

namespace Project.Core
{
    public class PlayerAnimationsSystem : Actor, IPlayerAnimationsService
    {
        [Space(10)]
        [SerializeField] private Animator _playerAnimatorController;
        
        private PlayerAnimationsSystem() { }

        public Animator PlayerAnimatorController => _playerAnimatorController;
        
        public void PlayTriggerAnimation(AnimationType animationType)
        {
            _playerAnimatorController.SetTrigger(animationType.ToString());
        }

        public void SetBoolParameter(AnimationType animationType, bool state)
        {
            _playerAnimatorController.SetBool(animationType.ToString(), state);
        }
    }
}
