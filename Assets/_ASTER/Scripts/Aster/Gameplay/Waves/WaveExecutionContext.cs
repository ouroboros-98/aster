using System.Collections.Generic;
using Aster.Core;
using Aster.Entity.Enemy;
using Aster.Utils;
using Aster.Utils.Pool;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    public class WaveExecutionContext
    {
        public readonly  int               WaveIndex;
        public readonly  IWaveElement      Previous;
        private readonly EnemySpawner      Spawner;
        public readonly  TowerAdderManager TowerAdder;

        public WaveExecutionContext(LevelDependencies dependencies, int waveIndex, IWaveElement previous)
        {
            Spawner    = dependencies.EnemySpawner;
            WaveIndex  = waveIndex;
            Previous   = previous;
            TowerAdder = dependencies.TowerAdder;
        }

        public void SpawnEnemy(Angle angle, List<EnemyController> enemies = null)
        {
            var enemy = EnemyPool.Instance.Get();
            enemy = Spawner.SpawnEnemy(angle);
            enemies?.Add(enemy);
        }

        public void SpawnEnemy(Angle angle, EnemyController prefab, List<EnemyController> enemies = null)
        {
            EnemyController enemy = Object.Instantiate(prefab);
            enemy = Spawner.SpawnEnemy(angle, enemy);
            enemies?.Add(enemy);
        }
    }
}