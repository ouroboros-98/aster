using _ASTER.Scripts.Aster.Entity.Enemy;
using Aster.Core.UI.EnemyIndicator;

namespace Aster.Core.InputSystem
{
    using System.Collections;
    using UnityEngine;

    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private EnemySpawner[] spawners;
        [SerializeField] private float          initialSpawnInterval     = 5f;
        [SerializeField] private int            initialEnemies           = 5;
        [SerializeField] private float          difficultyFactor         = 0.9f;
        [SerializeField] private int            additionalEnemiesPerWave = 2;

        private WaveData currentWave = new WaveData();

        private void Start()
        {
            currentWave.WaveNumber     = 1;
            currentWave.EnemiesToSpawn = initialEnemies;
            currentWave.SpawnInterval  = initialSpawnInterval;
            StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            while (true)
            {
                yield return StartCoroutine(SpawnWave());
                currentWave.WaveNumber++;
                currentWave.EnemiesToSpawn += additionalEnemiesPerWave;
                currentWave.SpawnInterval  *= difficultyFactor;
            }
        }

        private IEnumerator SpawnWave()
        {
            int enemiesLeft = currentWave.EnemiesToSpawn;

            // Pick a random starting index
            int startIndex = Random.Range(0, spawners.Length);

            // Choose up to three consecutive spawners
            int endIndex = Mathf.Min(startIndex + 2, spawners.Length - 1);

            while (enemiesLeft > 0)
            {
                for (int i = startIndex; i <= endIndex && enemiesLeft > 0; i++)
                {
                    spawners[i].Spawn();
                    enemiesLeft--;
                    yield return new WaitForSeconds(currentWave.SpawnInterval / currentWave.EnemiesToSpawn);
                }
            }
        }
    }
}