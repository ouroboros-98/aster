using Aster.Core;
using Aster.Towers;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Aster.UI
{
    using UnityEngine;
    using TMPro;

    namespace Aster.UI
    {
        public class TowerPickUI : AsterMono
        {
            // The RectTransform of the panel that will be used as the fill.
            [SerializeField] private RectTransform energyPanel;

            // The TextMeshPro field to display the current energy value.
            // [SerializeField] private TMP_Text energyText;

            // Energy value for which the panel is considered full (100% height).
            [SerializeField] private int energyThreshold = 50;

            [SerializeField] private BaseTower model;

            [SerializeField] private RectTransform redPanel;
            [FormerlySerializedAs("CrossImage")] [SerializeField] private Image crossImage;

            // The original height of the panel when full.
            private float maxPanelHeight;

            // Current energy value.
            private                  float currentEnergy;
            [SerializeField] private float lerpSpeed        = 5f;
            private                  float _targetFillRatio = 0f;

            private void Awake()
            {
                if (energyPanel != null)
                {
                    maxPanelHeight = energyPanel.rect.height;
                    Vector2 size = energyPanel.sizeDelta;
                    energyPanel.sizeDelta = new Vector2(size.x, 0f);
                }
            }

            private void Update()
            {
                if (energyPanel != null)
                {
                    // Ease the current fill ratio toward the target fill ratio
                    float currentFill = energyPanel.sizeDelta.y / maxPanelHeight;
                    currentFill = Mathf.Lerp(currentFill, _targetFillRatio, lerpSpeed * Time.deltaTime);
                    Vector2 size = energyPanel.sizeDelta;
                    energyPanel.sizeDelta = new Vector2(size.x, currentFill * maxPanelHeight);
                }
            }

            public RectTransform GetRedPanel()
            {
                return redPanel;
            }
            public void SetCrossEnable(bool enable)
            {
                crossImage.enabled = enable;
            }

            public int GetEnergyThreshold()
            {
                return energyThreshold;
            }

            public BaseTower GetModel()
            {
                return model;
            }

            // This method is executed whenever a change is made in the Editor.
            private void OnValidate()
            {
                UpdateEnergyText();
            }

            private void UpdateEnergyText()
            {
                // if (energyText != null)
                // {
                //     energyText.text = energyThreshold.ToString();
                // }
            }

            public void SetEnergy(float energy)
            {
                debugPrint("SetEnergy " + energy);
                currentEnergy = Mathf.Max(energy, 0f);
                // Calculate the new target fill ratio based on energy
                _targetFillRatio = Mathf.Min(currentEnergy / energyThreshold, 1f);
            }
        }
    }
}