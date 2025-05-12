using System;
using System.Collections;
using Aster.Entity.Enemy;
using Aster.Gameplay.Waves;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Aster.Core
{
    public class NewWaveManager : AsterMono
    {
        public LevelData[] Levels;

        private int  _currentLevelIndex;
        private bool _checkEnemyDead = false;

        private LevelExecution _currentLevelExecution;

        public bool IsRunning { get; private set; }

        [SerializeField]
        [Title("Dependencies")]
        [HideLabel]
        private LevelDependencies dependencies = new();

        private void Awake()
        {
            _currentLevelIndex = -1;
        }

        private void StartLevels()
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

            if (_currentLevelExecution.Status != LevelExecution.LevelStatus.Done)
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
            _currentLevelExecution.Initialize(dependencies);
            _currentLevelExecution.Status = LevelExecution.LevelStatus.InProgress;

            return true;
        }

        private void OnEnable()
        {
            AsterEvents.Instance.OnGameStartComplete += StartLevels;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnGameStartComplete -= StartLevels;
        }
    }
}