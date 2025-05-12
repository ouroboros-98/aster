using Aster.Core;
using Aster.Entity.Player;
using Aster.UI.Aster.UI;
using Aster.Utils;
using TMPro;
using UnityEngine;

namespace Aster.UI
{
    [ExecuteAlways]
    public class TowerOptionsManager : AsterMono
    {
        private const float PLAYER_1_X = -480f;
        private const float PLAYER_2_X = 480f;

        [Header("Tower Options")] private bool                 isMovingUp, isMovingDown;
        [SerializeField]          private TowerPickUI[]        towerOptions;
        [SerializeField]          private TMP_Text             textMeshPro;
        [SerializeField]          private float                extraMargin = 20f; // Additional space on each side
        [SerializeField]          private PlayerEnergy         playerEnergy;
        [SerializeField]          private TowerOptionsAnimator towerOptionsAnimator;

        private void Awake()
        {
            // UpdateTowerPositions();
            // ValidateComponent(ref towerOptionsAnimator);

            playerEnergy = PlayerEnergy.Instance;
        }

        public void Initialize(PlayerController player)
        {
            int playerIndex = player.PlayerIndex;

            float x = playerIndex == 0 ? PLAYER_1_X : PLAYER_2_X;

            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);

            ValidateComponent(ref towerOptionsAnimator);

            if (towerOptions == null) towerOptions = GetComponentsInChildren<TowerPickUI>(true);

            towerOptionsAnimator.Initialize(player);
        }

        private void Start()
            //should be on start so it does not come before the actual light increase*
        {
            GameEvents.OnLightPointAdded   += IncrementEnergy;
            GameEvents.OnLightPointRemoved += IncrementEnergyInt;
        }

        private void OnDisable()
        {
            GameEvents.OnLightPointAdded   -= IncrementEnergy;
            GameEvents.OnLightPointRemoved -= IncrementEnergyInt;
        }

        private void IncrementEnergy(float energy)
        {
            textMeshPro.text = playerEnergy.GetPlayerEnergy().ToString();
            foreach (var towerOption in towerOptions)
            {
                debugPrint("Incrementing energy for tower option: " + towerOption.name);
                towerOption.SetEnergy(playerEnergy.GetPlayerEnergy());
            }
        }

        private void IncrementEnergyInt(int energy)
        {
            textMeshPro.text = playerEnergy.GetPlayerEnergy().ToString();
            foreach (var towerOption in towerOptions)
            {
                towerOption.SetEnergy(playerEnergy.GetPlayerEnergy());
            }
        }

        public TowerPickUI[] GetTowerOptions()
        {
            if (towerOptions == null) towerOptions = GetComponentsInChildren<TowerPickUI>(true);
            return towerOptions;
        }

        public void SetCrossEnable(bool enable)
        {
            foreach (var towerOption in towerOptions)
            {
                towerOption.SetCrossEnable(!enable);
            }
        }
    }
}