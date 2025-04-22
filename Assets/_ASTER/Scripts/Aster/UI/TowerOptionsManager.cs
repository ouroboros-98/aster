// File: `Assets/_ASTER/Scripts/Aster/UI/TowerOptionsManager.cs`

using Aster.Core.UI.Aster.Core.UI;
using Aster.Entity.Player;
using Aster.Light;
using TMPro;
using UnityEngine;

namespace Aster.Core.UI
{
    [ExecuteAlways]
    public class TowerOptionsManager : MonoBehaviour
    {
        [SerializeField] private TowerPickUI[] towerOptions;
        [SerializeField] private TMP_Text textMeshPro;
        [SerializeField] private float extraMargin = 20f; // Additional space on each side
        [SerializeField] private PlayerEnergy playerEnergy;
        private void Start()
        {
            AsterEvents.Instance.OnLightPointAdded += IncrementEnergy;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnLightPointAdded -= IncrementEnergy;

        }
        private void IncrementEnergy(LightPoint lightPoint)
        {
            textMeshPro.text = playerEnergy.GetPlayerEnergy().ToString();
            foreach (var towerOption in towerOptions)
            {
                towerOption.SetEnergy(playerEnergy.GetPlayerEnergy());
            }
        }
        public TowerPickUI[] GetTowerOptions()
        {
            return towerOptions;
        }
        
        private void OnValidate()
        {
            UpdateTowerPositions();
        }

        [ContextMenu("Update Tower Positions")]
        public void UpdateTowerPositions()
        {
            RectTransform parentRect = GetComponent<RectTransform>();
            if (parentRect == null || towerOptions == null || towerOptions.Length == 0)
                return;

            float parentWidth = parentRect.rect.width;
            if (parentWidth <= 0f)
            {
                Debug.LogWarning("Parent RectTransform has zero or negative width.");
                return;
            }

            int count = towerOptions.Length;
            // Calculate usable width after subtracting extra margins.
            float usableWidth = parentWidth - 2 * extraMargin;
            
            for (int i = 0; i < count; i++)
            {
                RectTransform childRect = towerOptions[i].GetComponent<RectTransform>();
                if (childRect != null)
                {
                    // Calculate X position using extraMargin and usable width
                    float xPos = -parentWidth * 0.5f + extraMargin + usableWidth * ((i + 0.5f) / count);
                    Vector2 anchoredPos = childRect.anchoredPosition;
                    anchoredPos.x = xPos;
                    childRect.anchoredPosition = anchoredPos;
                }
            }
        }
    }
}