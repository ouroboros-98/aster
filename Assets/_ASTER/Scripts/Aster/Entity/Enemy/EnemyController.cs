using System;
using System.Data;
using _ASTER.Scripts.Aster.Entity.Enemy;
using Aster.Core;
using Aster.Core.Entity;
using Aster.Entity.StateMachine;
using Aster.Light;
using Aster.Utils.Pool;
using Mono.Cecil;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Entity.Enemy
{
    public class EnemyController : BaseEntityController, IPoolable
    {
        [SerializeField] private int damagePerLightHit = 1; // amount of HP removed per light hit

        protected override void Awake()
        {
            base.Awake();

            Reset();
        }

        protected override void SetupStateMachine()
        {
            StateMachine = new();

            var moveState   = new EntityMoveState(this);
            var attackState = new EntityAttackState(this);
            At(attackState, moveState,  When(() =>
            {
                float distance = Vector3.Distance(transform.position, MainLightSource.Instance.transform.position);
                return distance > 1.2f;
            }));
            At(moveState, moveState, When(() => false));
            At(moveState, attackState,  When(() =>
            {
                float distance = Vector3.Distance(transform.position, MainLightSource.Instance.transform.position);
                return distance <= 1.2f;
            }));
            
            StateMachine.SetState(moveState);
        }

        public void Reset()
        {
            var movementProvider = new PrimitiveEnemyMovementProvider(transform);
            movementProvider.SetTarget(MainLightSource.Instance.transform);
            hp.Set(hp.MaxHP);
            movement.Init(rb, movementProvider);

            var attackProvider = new PrimitiveEnemyAttackProvider();
            attack.Init(attackProvider);
            attack.damage = 1;  // for example, resetting damage
            attack.initialTimeToAttack = 3f; 
            
        }

        public void LightHit()
        {
            hp.ChangeBy(-damagePerLightHit);
            if (hp <= 0)
            {
                EnemyPool.Instance.Return(this);
            }
        }
    }
}