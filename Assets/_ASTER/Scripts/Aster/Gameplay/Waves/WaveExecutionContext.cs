using Aster.Entity.Enemy;

namespace Aster.Gameplay.Waves
{
    public class WaveExecutionContext
    {
        public readonly int          WaveIndex;
        public readonly IWaveElement Previous;
        public readonly EnemySpawner Spawner;

        public WaveExecutionContext(EnemySpawner spawner, int waveIndex, IWaveElement previous)
        {
            Spawner   = spawner;
            WaveIndex = waveIndex;
            Previous  = previous;
        }
    }
}