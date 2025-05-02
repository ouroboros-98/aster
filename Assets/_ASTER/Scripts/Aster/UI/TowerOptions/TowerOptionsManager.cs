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
        [Header("Tower Options")] private bool          isMovingUp, isMovingDown;
        [SerializeField]          private TowerPickUI[] towerOptions;
        [SerializeField]          private TMP_Text      textMeshPro;
        [SerializeField]          private float         extraMargin = 20f; // Additional space on each side
        [SerializeField]          private PlayerEnergy  playerEnergy;

        private void Awake()
        {
            // UpdateTowerPositions();
            playerEnergy = PlayerEnergy.Instance;
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

        private void IncrementEnergy(int energy)
        {
            textMeshPro.text = playerEnergy.GetPlayerEnergy().ToString();
            foreach (var towerOption in towerOptions)
            {
                Debug.Log("Incrementing energy for tower option: " + towerOption.name);
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