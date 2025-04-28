using System.Collections;
using Aster.Entity.Enemy;
using Aster.Gameplay.Waves;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Aster.Core
{
    public class NewWaveManager : MonoBehaviour
    {
        [SerializeField] private BaseWave     tutorialWave;
        [SerializeField] private BaseWave     obstacleTutorialWave;
        [SerializeField] private BaseWave[]   waveTypes;
        [SerializeField] private EnemySpawner spawner;

        [SerializeField]         float _timeBetweenWaves;
        private                  int   _numOfEnemies = 1;
        private                  int   waveIndex     = -1;
        private                  int   _currentEnemies;
        [SerializeField] private int   enemiesToAdd = 2;

        private void Start()
        {
            tutorialWave.Init(spawner);

            obstacleTutorialWave.Init(spawner);

            foreach (var waveType in waveTypes)
            {
                waveType.Init(spawner);
            }

            StartNewWave(1);
            AsterEvents.Instance.OnWaveStart?.Invoke(1);
        }

        public void OnEnable()
        {
            AsterEvents.Instance.OnEnemyDeath += CheckEnemyCounter;
            AsterEvents.Instance.OnWaveStart  += UpdateCurrentEnemies;
            AsterEvents.Instance.OnWaveEnd    += StartNewWave;
        }

        public void OnDisable()
        {
            AsterEvents.Instance.OnEnemyDeath -= CheckEnemyCounter;
            AsterEvents.Instance.OnWaveStart  -= UpdateCurrentEnemies;
            AsterEvents.Instance.OnWaveEnd    -= StartNewWave;
        }

        private void CheckEnemyCounter(Vector3 obj)
        {
            _currentEnemies--;
            if (_currentEnemies <= 0)
            {
                AsterEvents.Instance.OnWaveEnd?.Invoke(1);
            }
        }

        private void StartNewWave(int obj)
        {
            waveIndex++;
            if (waveIndex > 1)
            {
                _numOfEnemies += enemiesToAdd;
                //get random waveType
                BaseWave randomWaveType = waveTypes[Random.Range(0, waveTypes.Length)];

                StartCoroutine(StartWaveAfterSeconds(_timeBetweenWaves, randomWaveType));
            }
            else
            {
                if (waveIndex == 0)
                {
                    tutorialWave.OnWaveStart();
                    UpdateCurrentEnemies(1);
                }

                if (waveIndex == 1)
                {
                    StartCoroutine(StartWaveAfterSeconds
                                       (_timeBetweenWaves, obstacleTutorialWave));
                }
            }
        }

        private void UpdateCurrentEnemies(int obj)
        {
            _currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        }

        private IEnumerator StartWaveAfterSeconds(float second, BaseWave waveType)
        {
            yield return new WaitForSeconds(second);
            waveType.UpdateNumOfEnemies(_numOfEnemies);
            waveType.OnWaveStart();
            AsterEvents.Instance.OnWaveStart?.Invoke(1);
        }
    }
}