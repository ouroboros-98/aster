using Aster.Core;
using Aster.Entity.Enemy;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [CreateAssetMenu(fileName = "BasicWave", menuName = "Waves/Basic Wave")]
    public class BaseWave : ScriptableObject
    {
        [SerializeField] protected int          NumOfEnemies;
        [SerializeField] protected float MinAngle; 
        [SerializeField] protected float MaxAngle;
        [SerializeField] protected float delay;
        [SerializeField] protected SpawnType spawnType;

        protected EnemySpawner EnemySpawner;

        public void OnWaveStart(EnemySpawner spawner)
        {
            EnemySpawner = spawner;
            for (int i = 0; i < NumOfEnemies; i++)
            {
                EnemySpawner.SpawnEnemy(Random.Range(MinAngle, MaxAngle));
            }
        }

        public virtual void OnWaveEnd()
        {
            AsterEvents.Instance.OnWaveEnd?.Invoke(1);
        }

        public void UpdateNumOfEnemies(int numOfEnemies)
        {
            NumOfEnemies = numOfEnemies;
        }

        public SpawnType GetSpawnType()
        {
            return spawnType;
        }
        
        public float getDelay() { return delay; }
    }
}