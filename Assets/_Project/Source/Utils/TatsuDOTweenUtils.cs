using DG.Tweening;
using UnityEngine;

namespace Kaardik.Core
{
    public static class TatsuDOTweenUtils
    {
        public static void FadeCanvasGroup(CanvasGroup canvasGroup, float fadeValue, bool interactableState, float fadeDuration = 0.1f)
        {
            canvasGroup.DOFade(fadeValue, fadeDuration);
            canvasGroup.interactable = canvasGroup.blocksRaycasts = interactableState;
        }

        public static void ShakeUIObject(Transform transform, float duration, float shakeAmount,
            int vibrato, float randomness)
        {
            transform.DOShakePosition(duration, shakeAmount, vibrato, randomness);
        }
    }
}
