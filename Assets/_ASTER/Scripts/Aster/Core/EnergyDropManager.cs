using System;
using Aster.Entity.Enemy;
using Aster.Utils.Pool;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;


namespace Aster.Core
{
    public class EnergyDropManager : AsterMono
    {
        [SerializeField, Range(0f, 1f)]
        private float chanceToDrop;
        
        [SerializeField] private GameObject lightPointPrefab;


        private void OnEnable()
        {
            AsterEvents.Instance.OnEnemyDeath += SpawnEnergyOnEnemyDeath;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnEnemyDeath -= SpawnEnergyOnEnemyDeath;
        }


        private void SpawnEnergyOnEnemyDeath(EnemyController enemy)
        {
            if (!enemy.CurrentStateName.Equals("EntityAttackState"))
            {
                if (Random.value > chanceToDrop)
                    return;

                int pointNum = Random.Range(1, 5);
                for (int i = 0; i < pointNum; i++)
                {
                    Vector3 startPos = enemy.transform.position;
                    Vector2 offset = Random.insideUnitCircle * 1.5f;
                    Vector3 targetPos = startPos + new Vector3(offset.x, 0f, offset.y);

                    var energy = EnergyPool.Instance.Get(startPos, Quaternion.identity);
                    energy.transform.DOMove(targetPos, 0.5f).SetEase(Ease.OutCubic)
                        .OnComplete(energy.ResetMovement);
                }
            }
        }

    }
}