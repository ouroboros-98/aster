using System;
using Aster.Entity;
using Aster.Entity.StateMachine;
using Aster.Core;
using Aster.Core.Entity;
using Aster.StateMachine;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Entity
{
    public abstract class BaseEntityController : BaseStateMachineUser<IEntityState, EntityStateMachine>
    {
        [SerializeField, Label("Rigidbody")] protected Rigidbody rb;

        [SerializeField, BoxedProperty, Label("HP")] protected EntityHP       hp;
        [SerializeField, BoxedProperty] protected EntityMovement movement;

        public EntityHP       HP       => hp;
        public EntityMovement Movement => movement;

        protected override void Awake()
        {
            ValidateComponent(ref rb);
            base.Awake();
        }
    }
}