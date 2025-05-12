using Aster.Core;
using Aster.Core.InputSystem;
using Aster.Entity.Enemy;
using Aster.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace _ASTER.Scripts.Aster.Core
{
    public class EndScreenUICollector : AsterSingleton<EndScreenUICollector>
    {
        private int waveNumber;
        private int highScore;

        [SerializeField]
        private TextMeshProUGUI textWaveNumber;

        [SerializeField]
        private TextMeshProUGUI textWaveHighScore;

        [SerializeField]
        private Image imageWaveHighScore;

        [SerializeField]
        private Image imageWaveNumber;

        [SerializeField]
        private float fadeDuration = 2f; // Adjustable fade in time

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            AsterEvents.Instance.OnWaveEnd += OnWaveEnd;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // This method gets invoked after a new scene is loaded.
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Update UI references here.
            // For example, assuming the new scene has objects with these names.
            if (scene.name == "EndScreen")
            {
                textWaveNumber     = GameObject.Find("TextWaveNumber")?.GetComponent<TextMeshProUGUI>();
                textWaveHighScore  = GameObject.Find("TextWaveHighScore")?.GetComponent<TextMeshProUGUI>();
                imageWaveNumber    = GameObject.Find("ImageWaveNumber")?.GetComponent<Image>();
                imageWaveHighScore = GameObject.Find("ImageWaveHighScore")?.GetComponent<Image>();
                // Optionally trigger a fade-in in the new scene.
                if (textWaveNumber != null)
                    textWaveNumber.text = waveNumber.ToString();
                if (highScore <= waveNumber)
                {
                    highScore = waveNumber;
                    if (textWaveHighScore != null)
                        textWaveHighScore.text = highScore.ToString();
                }

                waveNumber = 0;
                FadeInAll();
            }
        }

        private void FadeInAll()
        {
            if (textWaveNumber != null)
            {
                Color textColor = textWaveNumber.color;
                textColor.a          = 0f;
                textWaveNumber.color = textColor;
                textWaveNumber.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }

            if (textWaveHighScore != null)
            {
                Color textColor2 = textWaveHighScore.color;
                textColor2.a            = 0f;
                textWaveHighScore.color = textColor2;
                textWaveHighScore.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }

            if (imageWaveNumber != null)
            {
                Color imageColor = imageWaveNumber.color;
                imageColor.a          = 0f;
                imageWaveNumber.color = imageColor;
                imageWaveNumber.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }

            if (imageWaveHighScore != null)
            {
                Color imageColor2 = imageWaveHighScore.color;
                imageColor2.a            = 0f;
                imageWaveHighScore.color = imageColor2;
                imageWaveHighScore.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }
        }

        private void OnWaveEnd()
        {
            this.waveNumber++;
        }
    }
}