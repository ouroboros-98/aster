using _ASTER.Scripts.Aster.Entity.Enemy;
using Aster.Core;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    public abstract class BaseWave : ScriptableObject
    {
        protected                  EnemySpawner EnemySpawner;
        [SerializeField] protected int          NumOfEnemies;

        public void Init(EnemySpawner enemySpawner)
        {
            EnemySpawner = enemySpawner;
        }

        public abstract void OnWaveStart();

        public virtual void OnWaveEnd()
        {
            AsterEvents.Instance.OnWaveEnd?.Invoke(1);
        }

        public void UpdateNumOfEnemies(int numOfEnemies)
        {
            NumOfEnemies = numOfEnemies;
        }
    }
}