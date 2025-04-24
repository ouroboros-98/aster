// File: `Assets/_ASTER/Scripts/Aster/Entity/Player/TowerBuying.cs`

using Aster.Core;
using Aster.Core.UI;
using Aster.Towers;
using DependencyInjection;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class TowerBuying : MonoBehaviour
    {
        [SerializeField] private TowerOptionsManager towerOptionsManager;
        [SerializeField] private PlayerEnergy        playerEnergy;
        [Inject] private InputHandler inputHandler;
        private Transform spawnPoint;
        private int       currentIndex;

        private void OnEnable()
        {
            // Subscribe to events from InputHandler
            if (inputHandler != null)
            {
                inputHandler.OnR1 += OnR1Performed;
                inputHandler.OnL1 += OnL1Performed;
                inputHandler.OnSelectTower += OnSelectTowerPerformed;
            }
        }

        private void OnDisable()
        {
            if (inputHandler != null)
            {
                inputHandler.OnR1 -= OnR1Performed;
                inputHandler.OnL1 -= OnL1Performed;
                inputHandler.OnSelectTower -= OnSelectTowerPerformed;
            }
        }
        private void Start()
        {
            currentIndex = 0;
            SetActiveTower(currentIndex);
        }

        private void OnR1Performed()
        {
            currentIndex = (currentIndex + 1) % towerOptionsManager.GetTowerOptions().Length;
            SetActiveTower(currentIndex);
        }

        private void OnL1Performed()
        {
            currentIndex = (currentIndex - 1 + towerOptionsManager.GetTowerOptions().Length)
                % towerOptionsManager.GetTowerOptions().Length;
            SetActiveTower(currentIndex);
        }

        private void OnSelectTowerPerformed()
        {
            var towerOption = towerOptionsManager.GetTowerOptions()[currentIndex];
            int cost = towerOption.GetEnergyThreshold();
            if (playerEnergy.GetPlayerEnergy() >= cost)
            {
                AsterEvents.Instance.OnLightPointRemoved?.Invoke(cost);
                spawnPoint = transform;
                // var newYSpawn= 0.5f;
                Instantiate(
                    towerOption.GetModel(),
                    spawnPoint.position,
                    spawnPoint.rotation
                );
            }
        }

        private void SetActiveTower(int index)
        {
            var towers = towerOptionsManager.GetTowerOptions();
            for (int i = 0; i < towers.Length; i++)
            {
                var redPanel = towers[i].GetRedPanel();
                if (redPanel) 
                    redPanel.gameObject.SetActive(i == index);
            }
        }
    }
}