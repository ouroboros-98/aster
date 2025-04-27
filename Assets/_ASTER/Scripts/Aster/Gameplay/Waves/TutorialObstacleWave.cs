using Aster.Utils;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [CreateAssetMenu(fileName = "TutorialObstacleWave", menuName = "Scriptable Objects/TutorialObstacleWave")]
    public class TutorialObstacleWave : BaseWave
    {
        public override void OnWaveStart()
        {
            Angle firstEnemyAngle = 0f;
            EnemySpawner.SpawnEnemy(firstEnemyAngle);
        }
    }
}