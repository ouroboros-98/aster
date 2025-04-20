using Aster.Core;
using Aster.Utils.Pool;
using UnityEngine;

namespace _ASTER.Scripts.Aster.Entity.Enemy
{
    public class EnemySpawner : AsterMono
    {
        public void Spawn()
        {
            var enemy = EnemyPool.Instance.Get();
            enemy.transform.position = transform.position;
            enemy.transform.rotation = transform.rotation;
        }
    }
}