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
        protected override void Awake()
        {
            base.Awake();

            Reset();
        }

        protected override void SetupStateMachine()
        {
            StateMachine = new();

            var moveState = new EntityMoveState(this);
            var attackState = new EntityAttackState(this);
            
            At(moveState, moveState, When(() => false));
            //At(moveState, attackState, When(); todo: do this condition

            StateMachine.SetState(moveState);
        }

        public void Reset()
        {
            var movementProvider = new PrimitiveEnemyMovementProvider(transform);
            movementProvider.SetTarget(MainLightSource.Instance.transform);

            movement.Init(rb, movementProvider);

            var attackProvider = new PrimitiveEnemyAttackProvider();
            attack.Init(attackProvider);
        }
    }
}