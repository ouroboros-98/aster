using System;
using Aster.Core.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aster.Core
{
    public class InputHandler
    {
        private static InputHandler _instance;

        public static InputHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InputHandler();
                }

                return _instance;
            }
        }

        public Aster_InputActions Actions { get; private set; }

        public Vector2 Movement => Actions.Player.Move.ReadValue<Vector2>();
        public Vector2 Rotation => Actions.Player.Rotate.ReadValue<Vector2>();

        public InputAction RotationInteraction => Actions.Player.RotationInteraction;
        public InputAction Cancel              => Actions.Player.Cancel;

        public event Action OnInteract;
        public event Action OnCancel;

        private InputHandler()
        {
            Actions = new Aster_InputActions();
            Actions.Enable();

            SetupEventBindings();
        }

        private void SetupEventBindings()
        {
            OnInteract = delegate { };
            OnCancel   = delegate { };

            SetupButtonBinding(Actions.Player.Interact, () => OnInteract);
            SetupButtonBinding(Actions.Player.Cancel,   () => OnCancel);
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