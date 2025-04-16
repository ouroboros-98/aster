using System;
using _ASTER.Scripts.Aster.Entity;
using _ASTER.Scripts.Aster.Entity.StateMachine;
using Aster.Core;
using Aster.Core.Entity;
using Aster.StateMachine;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Entity
{
    public abstract class BaseEntityController : BaseStateMachineUser<IEntityState, EntityStateMachine>
    {
        [SerializeField] protected EntityHP       hp;
        [SerializeField] protected EntityMovement movement;
        [SerializeField] protected Rigidbody      rb;

        public EntityHP       HP       => hp;
        public EntityMovement Movement => movement;

        protected override void Awake()
        {
            ValidateComponent(ref rb);
            base.Awake();
        }
    }
}