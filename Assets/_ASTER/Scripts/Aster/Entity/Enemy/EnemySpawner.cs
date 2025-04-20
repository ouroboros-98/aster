using Aster.Core;
using UnityEngine;

namespace _ASTER.Scripts.Aster.Entity.Enemy
{
    public class EnemySpawner : AsterMono
    {
        [SerializeField] private GameObject _enemyPrefab;
        public void Spawn()
        {
            Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}