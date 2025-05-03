using Aster.Core;
using Aster.Entity.Enemy;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Waves/New Level")]
    public class WavesLevel : ScriptableObject
    {
        public BaseWave[] waves;
        private int _currentWaveIndex;
        

        public SpawnType StartNextWave(EnemySpawner spawner)
        {
            if (_currentWaveIndex == waves.Length-1) // if last wave in level
            {
                waves[_currentWaveIndex++].OnWaveStart(spawner);
                return SpawnType.AfterEnemiesDead;
            }
            if (_currentWaveIndex == waves.Length)
            {
                AsterEvents.Instance.OnLevelEnd.Invoke(_currentWaveIndex);
                return SpawnType.AfterEnemiesDead;
            }
            waves[_currentWaveIndex].OnWaveStart(spawner);
            return waves[_currentWaveIndex++].GetSpawnType();
        }

        public float GetWaveDelay()
        {
            return waves[_currentWaveIndex].getDelay();
        }
    }
}