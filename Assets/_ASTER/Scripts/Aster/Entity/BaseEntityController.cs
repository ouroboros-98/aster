using System;
using _ASTER.Scripts.Aster.Entity;
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
        [SerializeField, BoxedProperty] protected EntityAttack attack;


        public EntityHP       HP       => hp;
        public EntityMovement Movement => movement;
        public EntityAttack Attack => attack; 

        protected override void Awake()
        {
            ValidateComponent(ref rb);
            base.Awake();
        }
    }
}