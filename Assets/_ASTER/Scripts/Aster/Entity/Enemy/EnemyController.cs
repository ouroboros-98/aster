using System.Data;
using _ASTER.Scripts.Aster.Entity.Enemy;
using Aster.Core;
using Aster.Core.Entity;
using Aster.Entity.StateMachine;
using Aster.Light;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Entity.Enemy
{
    public class EnemyController : BaseEntityController
    {
        protected override void Awake()
        {
            base.Awake();

            var movementProvider = new PrimitiveEnemyMovementProvider(transform);
            movementProvider.SetTarget(MainLightSource.Instance.transform);

            movement.Init(rb, movementProvider);
        }

        protected override void SetupStateMachine()
        {
            StateMachine = new();

            var moveState = new EntityMoveState(this);

            At(moveState, moveState, When(() => false));

            StateMachine.SetState(moveState);
        }
    }
}