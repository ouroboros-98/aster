using System;
using Aster.Core;
using Aster.Light;
using Aster.Utils;
using Aster.Utils.Pool;
using UnityEngine;

namespace Aster.Entity.Enemy
{
    public class EnemySpawner : AsterMono
    {
        [SerializeField] private float     spawnDistance;
        private                  int       _enemiesAlive = 0;
        private                  Transform mainLightSource;

        private void Awake()
        {
            mainLightSource = MainLightSource.Instance.transform;
        }

        private void OnEnable()
        {
            AsterEvents.Instance.OnEnemyDeath += UpdateEnemyCount;
        }

        private void UpdateEnemyCount(EnemyController enemy)
        {
            _enemiesAlive--;
            if (_enemiesAlive == 0)
            {
                AsterEvents.Instance.AllEnemiesDead.Invoke();
            }
        }


        public void Spawn()
        {
            var enemy = EnemyPool.Instance.Get();
            enemy.transform.position = transform.position;
            enemy.transform.rotation = transform.rotation;
            var indicator = IndicatorPool.Instance.Get();
            indicator.Init(enemy.transform);
            _enemiesAlive++;
        }

        public EnemyController SpawnEnemy(Angle direction)
        {
            direction -= 90;
            direction = 180 - direction;
            Vector3 spawnDirection = Quaternion.Euler(0, direction, 0) * Vector3.forward;
            Vector3 spawnPosition  = mainLightSource.position + (spawnDistance * spawnDirection);

            var enemy = EnemyPool.Instance.Get();
            enemy.transform.position = spawnPosition.With(y: Config.LightRayYPosition);

            GameEvents.OnEnemySpawn?.Invoke(enemy);
            
            return enemy;
        }
    }
}