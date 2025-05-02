using System.Collections;
using Aster.Entity.Enemy;
using Aster.Gameplay.Waves;
using UnityEngine;

namespace Aster.Core
{
    public class NewWaveManager : MonoBehaviour
    {
        public WavesLevel[] Levels;
        private int _currentLevelIndex;
        private bool _checkEnemyDead = false;

        [SerializeField] private EnemySpawner spawner;

        
        private void Start()
        {
            if (Levels.Length > 0 && Levels != null)
            {
                InitiateWave();
            }
        }

        private void OnEnable()
        {
            AsterEvents.Instance.OnLevelEnd += ChangeLevel;
            AsterEvents.Instance.AllEnemiesDead += InitiateWaveAfterDeath;
        }
        
        private void OnDisable()
        {
            AsterEvents.Instance.OnLevelEnd -= ChangeLevel;
            AsterEvents.Instance.AllEnemiesDead -= InitiateWaveAfterDeath;

        }

        private void ChangeLevel(int obj)
        {
            _checkEnemyDead = false;
            _currentLevelIndex++;
        }

        private void InitiateWave()
        {
            if (Levels[_currentLevelIndex].StartNextWave(spawner) == SpawnType.DuringPreviousRespawn)
            {
                StartCoroutine(InitiateWaveInDelay
                    (Levels[_currentLevelIndex].GetWaveDelay()));
            }
            else
            {
                _checkEnemyDead = true;
            }
        }

        private IEnumerator InitiateWaveInDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _currentLevelIndex++;
            InitiateWave();
        }
        
        private void InitiateWaveAfterDeath()
        {
            if (_checkEnemyDead)
            {
                _checkEnemyDead = false;
                InitiateWave();
            }
        }
    }
}