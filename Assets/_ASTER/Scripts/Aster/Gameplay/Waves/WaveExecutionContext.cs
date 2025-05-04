using System.Collections.Generic;
using Aster.Core;
using Aster.Entity.Enemy;
using Aster.Utils;

namespace Aster.Gameplay.Waves
{
    public class WaveExecutionContext
    {
        public readonly  int               WaveIndex;
        public readonly  IWaveElement      Previous;
        private readonly EnemySpawner      Spawner;
        public readonly  TowerAdderManager TowerAdder;

        public WaveExecutionContext(EnemySpawner      spawner, int waveIndex, IWaveElement previous,
                                    TowerAdderManager towerAdder)
        {
            Spawner    = spawner;
            WaveIndex  = waveIndex;
            Previous   = previous;
            TowerAdder = towerAdder;
        }

        public void SpawnEnemy(Angle angle, List<EnemyController> enemies = null)
        {
            var enemy = Spawner.SpawnEnemy(angle);
            enemies?.Add(enemy);
        }
    }
}