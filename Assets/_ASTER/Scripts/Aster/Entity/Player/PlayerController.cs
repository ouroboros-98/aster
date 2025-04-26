using System;
using Aster.Core;
using Aster.Entity.StateMachine;
using Aster.Entity;
using Aster.Entity.Player.States;
using DependencyInjection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Entity.Player
{
    public class PlayerController : BaseEntityController
    {
        [SerializeField]                                            private PlayerInteractor interactor;
        [FormerlySerializedAs("anchorControlelr")] [SerializeField] private PlayerAnchor     anchorController;

        [Inject] private InputHandler input;

        private ITargetMovementProvider movementProvider;

        private bool ReturnToBaseState => !interactor.IsInteracting;

        protected override void Awake()
        {
            Reset();

            base.Awake();

            movementProvider = new PlayerMovementProvider(input);
            movement.Init(rb, movementProvider);
        }

        protected override void SetupStateMachine()
        {
            StateMachine = new();

            var moveState        = new EntityMoveState(this);
            var interactionState = new PlayerInteractionState(this, interactor);
            var anchorState      = new PlayerAnchorState(this, anchorController, rb);

            At(moveState, interactionState, When(() => interactor.IsInteracting));

            At(interactionState, anchorState,      When(anchorController.IsAnchoring));
            At(anchorState,      interactionState, When(() => !anchorController.IsAnchoring()));

            Any(moveState, When(() => ReturnToBaseState));

            StateMachine.SetState(moveState);
        }

        private void Reset()
        {
            ValidateComponent(ref interactor, children: true);
            ValidateComponent(ref anchorController);
        }
    }
}