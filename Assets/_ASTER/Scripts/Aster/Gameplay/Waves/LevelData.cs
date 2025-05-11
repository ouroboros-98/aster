using System;
using System.Collections.Generic;
using System.Linq;
using Aster.Core;
using Aster.Entity.Enemy;
using Aster.Gameplay.Obstacles;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TNRD;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Waves/New Level")]
    public class LevelData : ScriptableObject
    {
        [Title("Obstacles")]
        [SerializeField]
        private bool spawnObstacles = false;

        [SerializeField]
        [ShowIf("spawnObstacles")]
        [TableList(AlwaysExpanded = true)]
        private List<ObstacleSpawnData> obstacles;

        [Space]
        [HorizontalGroup("Tower", Title = "Tower")]
        [SerializeField]
        [LabelText("Spawn")]
        private bool spawnTower = false;

        [SerializeField]
        [ShowIf("spawnTower")]
        [HorizontalGroup("Tower")]
        [PreviewField(60, ObjectFieldAlignment.Right)]
        [HideLabel]
        private GameObject towerPrefab;

        [Title("Waves")]
        [Space]
        [SerializeReference, SerializeReferenceDropdown]
        private IWaveElement[] waves;

        public IReadOnlyList<IWaveElement>      Waves     => waves.ToList();
        public IReadOnlyList<ObstacleSpawnData> Obstacles => spawnObstacles ? obstacles : new List<ObstacleSpawnData>();

        public GameObject TowerPrefab => spawnTower ? towerPrefab : null;

        public void Reset()
        {
            foreach (IWaveElement wave in waves)
            {
                wave.Reset();
            }
        }
    }

    [Serializable]
    [InlineProperty]
    public class LevelDependencies
    {
        [SerializeField]
        private EnemySpawner enemySpawner;

        [SerializeField]
        private TowerAdderManager towerAdder;

        [SerializeField]
        private ObstacleSpawnerManager obstacleSpawner;

        public EnemySpawner           EnemySpawner    => enemySpawner;
        public TowerAdderManager      TowerAdder      => towerAdder;
        public ObstacleSpawnerManager ObstacleSpawner => obstacleSpawner;
    }

    public class LevelExecution
    {
        public enum LevelStatus
        {
            NotStarted,
            InProgress,
            Done
        }

        private readonly LevelData _level;

        private LevelDependencies _dependencies;

        private LevelStatus _status;

        public LevelStatus Status
        {
            get => _status;
            set
            {
                if (_status == LevelStatus.NotStarted && value == LevelStatus.InProgress)
                {
                    OnStart();
                }
                else if (_status == LevelStatus.InProgress && value == LevelStatus.Done)
                {
                    OnEnd();
                }

                _status = value;
            }
        }

        public LevelExecution(LevelData level)
        {
            _level = level;
            _level.Reset();
            _status = LevelStatus.NotStarted;
        }

        public void Initialize(LevelDependencies dependencies)
        {
            _dependencies = dependencies;

            int          currentIndex = 0;
            IWaveElement previous     = null;

            _level.Reset();

            foreach (IWaveElement wave in _level.Waves)
            {
                WaveExecutionContext context =
                    new WaveExecutionContext(dependencies, currentIndex, previous);
                wave.PreStart(context);
                previous = wave;
                currentIndex++;
            }
        }

        public void OnStart()
        {
            _level.Obstacles.ToList().ForEach((data) => _dependencies.ObstacleSpawner.SpawnObstacle(data));

            if (_level.TowerPrefab == null) return;
            _dependencies.TowerAdder.EnqueueTower(_level.TowerPrefab);
        }

        public void Update()
        {
            if (_status != LevelStatus.InProgress) return;

            bool allDone = true;

            foreach (IWaveElement wave in _level.Waves)
            {
                if (wave.Status == WaveStatus.Done) continue;
                allDone = false;
                wave.Update();
            }

            if (allDone) Status = LevelStatus.Done;
        }

        public void OnEnd()
        {
            _dependencies.ObstacleSpawner.RemoveCurrentObstacles();
        }
    }
}