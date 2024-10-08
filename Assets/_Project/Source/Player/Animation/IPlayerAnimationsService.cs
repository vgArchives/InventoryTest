using Coimbra.Services;
using UnityEngine;

namespace Project.Core
{
    [RequiredService]
    public interface IPlayerAnimationsService : IService
    {
        public Animator PlayerAnimatorController { get; }

        public void PlayTriggerAnimation(AnimationType animationType);

        public void SetBoolParameter(AnimationType animationType, bool state);
    }
}