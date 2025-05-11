using Aster.Core;
using Aster.Entity.Enemy;
using Aster.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _ASTER.Scripts.Aster.Core
{
    public class EndScreenUICollector : AsterSingleton<EndScreenUICollector>
    {
        private int waveNumber;
        private int highScore;
        [SerializeField] private TextMeshProUGUI textWaveNumber;
        [SerializeField] private TextMeshProUGUI textWaveHighScore;
        [SerializeField] private Image imageWaveHighScore;
        [SerializeField] private Image imageWaveNumber;
        [SerializeField] private float fadeDuration = 2f; // Adjustable fade in time

        private void Awake()
        {
            AsterEvents.Instance.OnWaveEnd += OnWaveEnd;
            AsterEvents.Instance.OnLightSourceDestroyed += OnGameOver;
            FadeInAll();
        }

        private void FadeInAll()
        {
            if (textWaveNumber != null)
            {
                Color textColor = textWaveNumber.color;
                textColor.a = 0f;
                textWaveNumber.color = textColor;
                textWaveNumber.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }
            if (textWaveHighScore != null)
            {
                Color textColor2 = textWaveHighScore.color;
                textColor2.a = 0f;
                textWaveHighScore.color = textColor2;
                textWaveHighScore.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }
            if (imageWaveNumber != null)
            {
                Color imageColor = imageWaveNumber.color;
                imageColor.a = 0f;
                imageWaveNumber.color = imageColor;
                imageWaveNumber.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }
            if (imageWaveHighScore != null)
            {
                Color imageColor2 = imageWaveHighScore.color;
                imageColor2.a = 0f;
                imageWaveHighScore.color = imageColor2;
                imageWaveHighScore.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }
        }

        private void OnWaveEnd(int waveNumber)
        {
            this.waveNumber = waveNumber;
        }

        private void OnGameOver()
        {
            textWaveNumber.text = waveNumber.ToString();
            if (highScore >= waveNumber) return;
            highScore = waveNumber;
            textWaveHighScore.text = highScore.ToString();
            waveNumber = 0;
        }
    }
}