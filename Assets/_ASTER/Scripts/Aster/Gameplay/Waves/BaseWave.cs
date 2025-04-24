using _ASTER.Scripts.Aster.Entity.Enemy;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [CreateAssetMenu(fileName = "BaseWave", menuName = "Scriptable Objects/BaseWave")]
    public abstract class BaseWave : ScriptableObject
    {
        protected EnemySpawner EnemySpawner;

        public void Init(EnemySpawner enemySpawner)
        { 
            EnemySpawner = enemySpawner;
        }
        public abstract void OnWaveStart();
        public abstract void OnWaveEnd();
    }
}
