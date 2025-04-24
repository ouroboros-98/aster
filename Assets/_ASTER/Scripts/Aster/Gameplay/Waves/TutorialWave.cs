
using Aster.Utils;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [CreateAssetMenu(fileName = "TutorialWave", menuName = "Scriptable Objects/TutorialWave")]
    public class TutorialWave : BaseWave
    {
        public override void OnWaveStart()
        {
            Angle firstEnemyAngle = 270f;
            EnemySpawner.SpawnEnemy(firstEnemyAngle);
        }

        public override void OnWaveEnd()
        {
            return; 
        }
    }
}
