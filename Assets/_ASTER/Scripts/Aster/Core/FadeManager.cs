using System;
using Aster.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _ASTER.Scripts.Aster.Core
{
    public class FadeManager : AsterMono
    {
        [SerializeField]
        private Image fadeImage; // Assign in Inspector

        [SerializeField]
        private float fadeDuration = 2f;

        public void FadeOut(Action OnComplete)
        {
            if (fadeImage != null)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);

                fadeImage.DOFade(1f, fadeDuration)
                         .SetEase(Ease.Linear)
                         .OnComplete(() => OnComplete());
            }
            else
            {
                Debug.LogWarning("Fade image not assigned in EndScreenManager.");
            }
        }

        public void FadeIn(Action OnComplete)
        {
            if (fadeImage != null)
            {
                SetFadeImageToBlack();

                fadeImage.DOFade(0, fadeDuration)
                         .SetEase(Ease.Linear)
                         .OnComplete(() => OnComplete());
            }
            else
            {
                Debug.LogWarning("Fade image not assigned in EndScreenManager.");
            }
        }

        public void SetFadeImageToBlack()
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
        }
    }
}