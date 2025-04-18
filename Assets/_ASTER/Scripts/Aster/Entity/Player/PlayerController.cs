using System;
using Aster.Core;
using Aster.Entity.StateMachine;
using Aster.Entity;
using Aster.Entity.Player.States;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class PlayerController : BaseEntityController
    {
        [SerializeField] private PlayerInteractor interactor;

        private ITargetMovementProvider movementProvider;

        private bool ReturnToBaseState => !interactor.IsInteracting;

        protected override void Awake()
        {
            Reset();

            base.Awake();

            movementProvider = new PlayerMovementProvider();
            movement.Init(rb, movementProvider);
        }

        protected override void SetupStateMachine()
        {
            StateMachine = new();

            var moveState        = new EntityMoveState(this);
            var interactionState = new PlayerInteractionState(this, interactor);

            At(moveState, interactionState, When(() => interactor.IsInteracting));

            Any(moveState, When(() => ReturnToBaseState));

            StateMachine.SetState(moveState);
        }

        private void Reset()
        {
            ValidateComponent(ref interactor, children: true);
        }
    }
}