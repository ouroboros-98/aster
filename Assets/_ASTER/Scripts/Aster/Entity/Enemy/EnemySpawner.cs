using System;
using Aster.Core;
using Aster.Entity.Enemy;
using Aster.Light;
using Aster.Utils;
using Aster.Utils.Pool;
using UnityEngine;

namespace _ASTER.Scripts.Aster.Entity.Enemy
{
    public class EnemySpawner : AsterMono
    {
        [SerializeField] private float spawnDistance;

        private Transform              mainLightSource;
        private IPool<EnemyController> _enemyPool;

        private void Awake()
        {
            _enemyPool      = EnemyPool.Instance;
            mainLightSource = MainLightSource.Instance.transform;
        }


        public void Spawn()
        {
            var enemy = EnemyPool.Instance.Get();
            enemy.transform.position = transform.position;
            enemy.transform.rotation = transform.rotation;
        }

        public void SpawnEnemy(Angle direction)
        {
            Vector3 spawnDirection = direction.ToVector3();
            Vector3 spawnPosition  = mainLightSource.position + (spawnDistance * spawnDirection);

            var enemy = EnemyPool.Instance.Get();
            enemy.transform.position = spawnPosition;
        }
    }
}