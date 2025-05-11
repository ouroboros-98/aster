using Aster.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace _ASTER.Scripts.Aster.Core
{
    public class EndScreenManager : MonoBehaviour
    {
        [SerializeField] private Image fadeImage; // Assign in Inspector
        [SerializeField] private float fadeDuration = 2f;
        [SerializeField] private string nextSceneName = "EndScreen";
        private bool isMoving = false;

        private void OnEnable()
        {
            AsterEvents.Instance.OnLightSourceDestroyed += DarkenScreen;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnLightSourceDestroyed -= DarkenScreen;
        }

        [Button("Do Fade Out")]
        private void DarkenScreen()
        {
            if (isMoving) return;
            isMoving = true;
            if (fadeImage != null)
            {
                fadeImage.DOFade(1f, fadeDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => SceneManager.LoadScene(nextSceneName));
            }
            else
            {
                Debug.LogWarning("Fade image not assigned in EndScreenManager.");
            }
        }
    }
}