using System;
using System.Collections;
using Aster.Entity.Enemy;
using Aster.Gameplay.Waves;
using UnityEngine;

namespace Aster.Core
{
    public class NewWaveManager : AsterMono
    {
        public  WavesLevel[] Levels;
        private int          _currentLevelIndex;
        private bool         _checkEnemyDead = false;

        private LevelExecution _currentLevelExecution;

        public bool IsRunning { get; private set; }

        [SerializeField] private EnemySpawner      spawner;
        [SerializeField] private TowerAdderManager towerAdder;

        private void Awake()
        {
            _currentLevelIndex = -1;
        }

        private void Start()
        {
            BeginLevels();
        }

        public void BeginLevels()
        {
            if (Levels == null || Levels.Length == 0)
            {
                errorPrint("No levels assigned to the WaveManager.");
                return;
            }

            IsRunning = true;
            NextLevel();
        }

        private void Update()
        {
            if (!IsRunning) return;
            if (_currentLevelExecution == null) return;

            if (!_currentLevelExecution.IsDone)
            {
                _currentLevelExecution.Update();
            }
            else NextLevel();
        }

        private bool NextLevel()
        {
            if (_currentLevelIndex >= Levels.Length - 1)
            {
                IsRunning = false;
                return false;
            }

            _currentLevelIndex++;
            _currentLevelExecution = new(Levels[_currentLevelIndex]);
            _currentLevelExecution.Initialize(spawner, towerAdder);

            return true;
        }
    }
}