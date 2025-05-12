using Aster.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace _ASTER.Scripts.Aster.Core
{
    public class EndScreenManager : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private FadeManager fadeManager;

        [SerializeField]
        private string nextSceneName = "EndScreen";

        private bool isMoving = false;

        private void OnEnable()
        {
            AsterEvents.Instance.OnLightSourceDestroyed += OnLightSourceDestroyed;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnLightSourceDestroyed -= OnLightSourceDestroyed;
        }

        private void OnLightSourceDestroyed()
        {
            if (isMoving) return;
            isMoving = true;
            fadeManager.FadeOut(() => SceneManager.LoadScene(nextSceneName));
        }
    }
}