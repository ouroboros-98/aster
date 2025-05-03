using System;
using Aster.Core;
using Aster.Entity.Player;
using Aster.Utils;
using DependencyInjection;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aster.Core
{
    public class PlayerRotationController : AsterMono
    {
        [SerializeField, Range(0, 1f)] private float inputSensitivity = .9f;

        #region FIELDS

        private RotationInteractionContext currentRotationInteraction;

        private Angle targetAngle;

        private PlayerController   player;
        private PlayerInputHandler InputHandler => player.PlayerInputHandler;

        #endregion

        #region PROPERTIES

        public bool  IsActive    => currentRotationInteraction != null;
        public Angle TargetAngle => targetAngle;

        private RotationHandler RotationHandler => currentRotationInteraction.Interactable.RotationHandler;

        #endregion

        #region EVENTS

        public event Action<RotationInteractionContext> OnInteractionBegin   = delegate { };
        public event Action<Angle>                      OnTargetAngleChanged = delegate { };
        public event Action                             OnDeactivate         = delegate { };

        #endregion

        public void Initialize(PlayerController player)
        {
            this.player = player;
        }

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
            if (rotationInteraction.Player != player) return;

            print("Activated");

            this.currentRotationInteraction = rotationInteraction;
            this.targetAngle                = RotationHandler.TargetAngle;

            OnInteractionBegin?.Invoke(currentRotationInteraction);

            if (Config.Targeting.RotateWithoutTargeting)
            {
                OnTargetAngleChanged += rotationInteraction.Interactable.RotationHandler.Rotate;
            }
        }

        private void Deactivate()
        {
            OnDeactivate?.Invoke();

            if (Config.Targeting.RotateWithoutTargeting)
            {
                OnTargetAngleChanged -= currentRotationInteraction.Interactable.RotationHandler.Rotate;
            }

            GameEvents.OnInteractionEnd?.Invoke(currentRotationInteraction);

            currentRotationInteraction = null;
        }

        public void Update()
        {
            if (!IsActive) return;

            if (InputHandler.Cancel.WasPressedThisFrame())
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
            return InputHandler.RotationInteraction.WasReleasedThisFrame();
        }

        private void UpdateAngle()
        {
            Vector2 targetDirection = GetTargetDirection();

            if (targetDirection.magnitude < inputSensitivity) return;

            Angle inputAngle      = targetDirection.ToAngle();
            Angle normalizedAngle = 90f - inputAngle;

            this.targetAngle = normalizedAngle;

            OnTargetAngleChanged?.Invoke(targetAngle);

            RotationHandler.ActiveTargetingAngle = targetAngle;
        }

        private Vector2 GetTargetDirection() => InputHandler.Rotation;

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