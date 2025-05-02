using System;
using System.Data;
using _ASTER.Scripts.Aster.Entity.Enemy;
using Aster.Core;
using Aster.Core.Entity;
using Aster.Entity.StateMachine;
using Aster.Light;
using Aster.Utils;
using Aster.Utils.Pool;
using Mono.Cecil;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Entity.Enemy
{
    public class EnemyController : BaseEntityController, IPoolable
    {
        [SerializeField] private int damagePerLightHit = 1; // amount of HP removed per light hit

        [SerializeField] private SerializableTimer invincibilityFrames = new(0.1f);

        private MainLightSource _mainLightSource;

        private float DistanceFromMainLight =>
            Vector3.Distance(transform.position, _mainLightSource.transform.position);

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

            At(attackState, moveState,   When(() => DistanceFromMainLight > 1.2f));
            At(moveState,   attackState, When(() => DistanceFromMainLight <= 1.2f));

            StateMachine.SetState(moveState);
        }

        public void Reset()
        {
            var movementProvider = new PrimitiveEnemyMovementProvider(transform);
            movementProvider.SetTarget(MainLightSource.Instance.transform);
            hp.Set(hp.MaxHP);
            movement.Init(rb, movementProvider);

            _mainLightSource = MainLightSource.Instance;

            var attackProvider = new PrimitiveEnemyAttackProvider();

            attack.Init(attackProvider, this);
            attack.damage              = 1; // for example, resetting damage
            attack.initialTimeToAttack = 3f;

            invincibilityFrames.Stop();
        }

        public void LightHit(LightHit hit)
        {
            hp.ChangeBy(-hit.Ray.Intensity * Time.fixedDeltaTime);

            if (hp > 0) return;

            AsterEvents.Instance.OnEnemyDeath?.Invoke(this.transform.position);

            EnemyPool.Instance.Return(this);
        }
    }
}