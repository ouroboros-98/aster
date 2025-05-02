// File: `Assets/_ASTER/Scripts/Aster/Entity/Player/TowerBuying.cs`

using System;
using Aster.Core;
using Aster.Core.UI;
using Aster.Towers;
using Aster.UI;
using DependencyInjection;
using Unity.Mathematics;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class TowerBuying : AsterMono
    {
        [SerializeField] private TowerOptionsManager towerOptionsManager;
        [SerializeField] private PlayerEnergy        playerEnergy;
        [Inject]         private InputHandler        inputHandler;
        private                  Transform           spawnPoint;
        private                  int                 currentIndex;
        private                 bool                 canBuy = true;
        private int triggersCount = 0;

        private void OnEnable()
        {
            towerOptionsManager = TowerOptionsManager.Instance;
            // Subscribe to events from InputHandler
            if (inputHandler != null)
            {
                inputHandler.OnR1          += OnR1Performed;
                inputHandler.OnL1          += OnL1Performed;
                inputHandler.OnSelectTower += OnSelectTowerPerformed;
            }
        }

        private void OnDisable()
        {
            if (inputHandler != null)
            {
                inputHandler.OnR1          -= OnR1Performed;
                inputHandler.OnL1          -= OnL1Performed;
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

            if (!canBuy||!playerEnergy.AttemptReduceEnergy(cost)) return;

            GameEvents.OnLightPointRemoved?.Invoke(cost);

            spawnPoint = transform;

            Instantiate(
                        towerOption.GetModel(),
                        spawnPoint.position,
                        quaternion.identity
                       );
        }
        public void SetCanBuy(bool value)
        {
            canBuy = value;
            towerOptionsManager.SetCrossEnable(value);
        }
        public void OnTriggerEntered()
        {
            triggersCount++;
            SetCanBuy(triggersCount == 0);
        }
        public void OnTriggerExited()
        {
            triggersCount = Mathf.Max(0, triggersCount - 1);
            SetCanBuy(triggersCount == 0);
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