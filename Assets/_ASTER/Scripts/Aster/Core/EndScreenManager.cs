using Aster.Core;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _ASTER.Scripts.Aster.Core
{
    public class EndScreenManager : MonoBehaviour
    {
        [SerializeField] private Image fadeImage; // Assign in Inspector
        [SerializeField] private float fadeDuration = 2f;

        private void OnEnable()
        {
            AsterEvents.Instance.OnLightSourceDestroyed += DarkenScreen;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnLightSourceDestroyed -= DarkenScreen;
        }

        private void DarkenScreen()
        {
            if (fadeImage != null)
            {
                fadeImage.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad);
            }
            else
            {
                Debug.LogWarning("Fade image not assigned in EndScreenManager.");
            }
        }
    }
}