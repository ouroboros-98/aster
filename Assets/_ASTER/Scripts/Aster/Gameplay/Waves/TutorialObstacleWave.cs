using Aster.Utils;

namespace Aster.Gameplay.Waves
{
    public class TutorialObstacleWave: BaseWave {
        public override void OnWaveStart()
        {
            Angle firstEnemyAngle = 0f;
            EnemySpawner.SpawnEnemy(firstEnemyAngle);
        }
    }
}