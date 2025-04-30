using System;
using Aster.Utils.Pool;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Aster.Core
{
    public class EnergyDropManager : AsterMono
    {
        [SerializeField, Range(0f, 1f)]
        private float chanceToDrop;
        
        [SerializeField] private GameObject lightPointPrefab;


        private void OnEnable()
        {
            AsterEvents.Instance.OnEnemyDeath += TrySpawnEnergyOnEnemyDeath;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnEnemyDeath -= TrySpawnEnergyOnEnemyDeath;
        }

        private void TrySpawnEnergyOnEnemyDeath(Vector3 enemyPos)
        {
            if (Random.value < chanceToDrop)
            {
                EnergyPool.Instance.Get(enemyPos, Quaternion.identity);
            }
        }
    }
}