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

        [SerializeField] private TextMeshProUGUI textWaveNumber;
        [SerializeField] private TextMeshProUGUI textWaveHighScore;
        [SerializeField] private TextMeshProUGUI textWaveNum;
        [SerializeField] private TextMeshProUGUI textHighNum;
        [SerializeField] private TextMeshProUGUI text1;
        [SerializeField] private TextMeshProUGUI text2;
        [SerializeField] private TextMeshProUGUI text3;
        [SerializeField] private float fadeDuration = 2f; // Adjustable fade in time

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
                textWaveNumber = GameObject.Find("Text4")?.GetComponent<TextMeshProUGUI>();
                textWaveHighScore = GameObject.Find("Text3")?.GetComponent<TextMeshProUGUI>();
                textWaveNum = GameObject.Find("Text1")?.GetComponent<TextMeshProUGUI>();
                textHighNum = GameObject.Find("Text2")?.GetComponent<TextMeshProUGUI>();
                text1 = GameObject.Find("Text5")?.GetComponent<TextMeshProUGUI>();
                text2 = GameObject.Find("Text6")?.GetComponent<TextMeshProUGUI>();
                text3 = GameObject.Find("Text7")?.GetComponent<TextMeshProUGUI>();
                
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
                Color c = textWaveNumber.color;
                c.a = 0f;
                textWaveNumber.color = c;
                textWaveNumber.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }

            if (textWaveHighScore != null)
            {
                Color c = textWaveHighScore.color;
                c.a = 0f;
                textWaveHighScore.color = c;
                textWaveHighScore.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }

            if (textWaveNum != null)
            {
                Color c = textWaveNum.color;
                c.a = 0f;
                textWaveNum.color = c;
                textWaveNum.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }

            if (textHighNum != null)
            {
                Color c = textHighNum.color;
                c.a = 0f;
                textHighNum.color = c;
                textHighNum.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }

            if (text1 != null)
            {
                Color c = text1.color;
                c.a = 0f;
                text1.color = c;
                text1.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }

            if (text2 != null)
            {
                Color c = text2.color;
                c.a = 0f;
                text2.color = c;
                text2.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }

            if (text3 != null)
            {
                Color c = text3.color;
                c.a = 0f;
                text3.color = c;
                text3.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            }
        }

        private void OnWaveEnd()
        {
            this.waveNumber++;
        }
        

       
    }
}