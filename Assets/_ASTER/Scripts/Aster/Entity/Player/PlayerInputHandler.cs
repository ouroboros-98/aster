using System;
using Aster.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aster.Entity.Player
{
    public class PlayerInputHandler : ITargetMovementProvider
    {
        public int PlayerIndex => _playerInput.playerIndex;

        private Vector2     _targetMovement;
        public  Vector2     TargetMovement => _playerInput.actions["Move"].ReadValue<Vector2>();
        private PlayerInput _playerInput;

        public Vector2      Rotation            => _playerInput.actions["Rotate"].ReadValue<Vector2>();
        public InputAction  RotationInteraction => _playerInput.actions["RotationInteraction"];
        public InputAction  Grab                => _playerInput.actions["Grab"];
        public InputAction  Cancel              => _playerInput.actions["Cancel"];
        public InputAction Quit => _playerInput.actions["Quit"];
        public float        RotationDirection   => _playerInput.actions["RotationDirection"].ReadValue<float>();
        public event Action OnInteract;
        public event Action OnCancel;
        public event Action OnTowerPicker_Right;
        public event Action OnTowerPicker_Left;
        public event Action OnPlaceTower;

        public PlayerInputHandler(PlayerInput playerInput)
        {
            _playerInput = playerInput;

            OnInteract          = delegate { };
            OnCancel            = delegate { };
            OnTowerPicker_Right = delegate { };
            OnTowerPicker_Left  = delegate { };
            OnPlaceTower        = delegate { };

            SetupButtonBinding(_playerInput.actions["Interact"],          () => OnInteract);
            SetupButtonBinding(_playerInput.actions["Cancel"],            () => OnCancel);
            SetupButtonBinding(_playerInput.actions["TowerPicker_Right"], () => OnTowerPicker_Right);
            SetupButtonBinding(_playerInput.actions["TowerPicker_Left"],  () => OnTowerPicker_Left);
            SetupButtonBinding(_playerInput.actions["PlaceTower"],        () => OnPlaceTower);
        }

        private static void SetupButtonBinding(InputAction inputAction, Func<Action> actionGetter)
        {
            inputAction.performed += context =>
                                     {
                                         Debug.Log($"Input action: {inputAction.name}. Performed: {context.performed}");

                                         if (context.performed)
                                         {
                                             actionGetter()?.Invoke();
                                         }
                                     };
        }
    }
}