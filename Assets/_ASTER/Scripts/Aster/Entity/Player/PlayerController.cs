using System;
using Aster.Core;
using Aster.Entity.StateMachine;
using Aster.Entity;
using Aster.Entity.Player.States;
using Aster.UI;
using Aster.Utils;
using DependencyInjection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Aster.Entity.Player
{
    public class PlayerController : BaseEntityController
    {
        [SerializeField]
        private Transform pivot;

        [SerializeField]
        private PlayerInteractor interactor;

        [SerializeField]
        private PlayerAnchor anchorController;

        [SerializeField]
        private PlayerRotationController rotationController;

        private PlayerGrabber _grabber;

        public PlayerGrabber Grabber
        {
            get
            {
                if (_grabber == null) _grabber = new(this, interactor);
                return _grabber;
            }
        }

        private PlayerInputHandler _playerInputHandler;
        public  PlayerInputHandler PlayerInputHandler => _playerInputHandler;

        private ITargetMovementProvider movementProvider;

        private bool                     ReturnToBaseState  => !interactor.IsInteracting;
        public  PlayerRotationController RotationController => rotationController;

        public int PlayerIndex => PlayerInputHandler.PlayerIndex;

        protected override void Awake()
        {
            Reset();

            _playerInputHandler = new PlayerInputHandler(GetComponent<PlayerInput>());

            gameObject.name = $"Player_{PlayerIndex}";

            // Create new empty GameObject
            Transform playerUIElements = new GameObject($"Player{PlayerIndex}_UI").transform;
            playerUIElements.transform.position = Vector3.zero;

            CreateRotationController(playerUIElements);
            // CreateTowerPickerUI(playerUIElements);

            interactor.enabled = true;

            base.Awake();

            movementProvider = _playerInputHandler;
            movement.Init(rb, movementProvider);
        }

        private void CreateTowerPickerUI(Transform parent)
        {
            var towerPickerUICanvas = Instantiate(Config.Entities.TowerPickerPrefab, parent, false);

            TowerOptionsManager towerUI = towerPickerUICanvas.GetComponentInChildren<TowerOptionsManager>();
            towerUI.Initialize(this);

            TowerBuying towerBuying = GetComponentInChildren<TowerBuying>();
            towerBuying.Initialize(this, towerUI);
        }

        private void CreateRotationController(Transform parent)
        {
            rotationController = Instantiate(Config.Entities.PlayerRotationControllerPrefab, parent, false);
            rotationController.Initialize(this);
        }

        protected override void SetupStateMachine()
        {
            StateMachine = new();

            var moveState        = new EntityMoveState(this);
            var interactionState = new PlayerInteractionState(this, interactor);
            var anchorState      = new PlayerAnchorState(this, anchorController, rb);

            var grabState = new PlayerGrabState(this, Grabber, movement);

            At(moveState, interactionState, When(() => interactor.IsInteracting));

            At(interactionState, anchorState,      When(anchorController.IsAnchoring));
            At(anchorState,      interactionState, When(() => !anchorController.IsAnchoring()));

            At(interactionState, grabState, When(() => Grabber.IsGrabbing));

            Any(moveState, When(() => ReturnToBaseState));

            StateMachine.SetState(moveState);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            pivot.position = pivot.position.With(y: Config.Entities.PlayerPivotY);
        }

        private void Reset()
        {
            ValidateComponent(ref interactor, children: true);
            ValidateComponent(ref anchorController);
        }

        public void Freeze()
        {
            rb.linearVelocity  = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic     = true;
        }

        public void Unfreeze()
        {
            rb.isKinematic = false;
        }
    }
}