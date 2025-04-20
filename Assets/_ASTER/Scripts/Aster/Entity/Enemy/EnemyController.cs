using System.Data;
using _ASTER.Scripts.Aster.Entity.Enemy;
using Aster.Core;
using Aster.Core.Entity;
using Aster.Entity.StateMachine;
using Aster.Light;
using Aster.Utils.Pool;
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

            var moveState = new EntityMoveState(this);

            At(moveState, moveState, When(() => false));

            StateMachine.SetState(moveState);
        }

        public void Reset()
        {
            var movementProvider = new PrimitiveEnemyMovementProvider(transform);
            movementProvider.SetTarget(MainLightSource.Instance.transform);
            hp.Set(hp.MaxHP);
            movement.Init(rb, movementProvider);
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