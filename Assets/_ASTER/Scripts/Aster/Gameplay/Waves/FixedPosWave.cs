using Aster.Utils;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    public class FixedPosWave : BaseWave
    {

        public override void OnWaveStart()
        {
            Angle startingAngle = Random.Range(0f, 360f);
            for (int i = 0; i < NumOfEnemies; i++)
            {
                EnemySpawner.SpawnEnemy(startingAngle + (i%4)*90);
            }
        }
    }
}