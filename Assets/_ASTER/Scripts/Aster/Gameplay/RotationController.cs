using System;
using Aster.Core;
using Aster.Utils;
using DependencyInjection;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aster.Core
{
    public class RotationController : AsterMono, IDependencyProvider
    {
        [SerializeField, Range(0, 1f)] private float inputSensitivity = .9f;

        #region FIELDS

        private RotationInteractionContext currentRotationInteraction;

        private Angle targetAngle;

        [Inject] private InputHandler inputHandler;

        #endregion

        #region PROPERTIES

        public bool IsActive => currentRotationInteraction != null;

        private RotationHandler RotationHandler => currentRotationInteraction.Interactable.RotationHandler;

        #endregion

        #region EVENTS

        public event Action<RotationInteractionContext> OnInteractionBegin   = delegate { };
        public event Action<Angle>                      OnTargetAngleChanged = delegate { };
        public event Action                             OnDeactivate         = delegate { };

        #endregion

        [Provide] public RotationController Instance() => this;

        public void OnEnable()
        {
            GameEvents.OnRotationInteractionBegin += Activate;
        }

        private void OnDisable()
        {
            GameEvents.OnRotationInteractionBegin -= Activate;
        }

        private void Activate(RotationInteractionContext rotationInteraction)
        {
            print("Activated");

            this.currentRotationInteraction = rotationInteraction;
            this.targetAngle                = RotationHandler.TargetAngle;

            OnInteractionBegin?.Invoke(currentRotationInteraction);
        }

        private void Deactivate()
        {
            OnDeactivate?.Invoke();

            GameEvents.OnInteractionEnd?.Invoke(currentRotationInteraction);

            currentRotationInteraction = null;
        }

        public void Update()
        {
            if (!IsActive) return;

            if (inputHandler.Cancel.WasPressedThisFrame())
            {
                Cancel();
                return;
            }

            if (IsButtonReleased())
            {
                Set();
                return;
            }

            UpdateAngle();
        }

        private bool IsButtonReleased()
        {
            return inputHandler.RotationInteraction.WasReleasedThisFrame();
        }

        private void UpdateAngle()
        {
            Vector2 targetDirection = GetTargetDirection();

            if (targetDirection.magnitude < inputSensitivity) return;

            Angle inputAngle      = targetDirection.ToAngle();
            Angle normalizedAngle = 90f - inputAngle;

            this.targetAngle = normalizedAngle;

            OnTargetAngleChanged?.Invoke(targetAngle);
        }

        private Vector2 GetTargetDirection() => inputHandler.Rotation;

        private void Set()
        {
            RotationHandler.Rotate(targetAngle);

            Deactivate();
        }

        private void Cancel()
        {
            Deactivate();
        }
    }
}