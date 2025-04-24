using System;
using Aster.Core;
using Aster.Light;
using Aster.Utils;
using DependencyInjection;
using UnityEngine;

namespace Aster.Towers
{
    public class EmitterTargetingRay : AsterMono
    {
        [Inject] private RotationController _rotationController;
        [Inject] private InputHandler       _inputHandler;

        private IRotatatble  _emitterRotatable;
        private IRotatatble  _currentRotatable;
        private TargetingRay _targetingRay;
        private Angle        _targetAngle;

        private bool IsTargetingRayActive => _targetingRay != null && _targetingRay.IsActive;

        private void Awake()
        {
            _emitterRotatable = GetComponent<IRotatatble>();
            _targetingRay     = null;
        }

        private void OnEnable()
        {
            GameEvents.OnRotationInteractionBegin += OnRotationInteractionBegin;
            GameEvents.OnInteractionEnd           += OnInteractionEnd;
        }

        private void OnDisable()
        {
            GameEvents.OnRotationInteractionBegin -= OnRotationInteractionBegin;
            GameEvents.OnInteractionEnd           -= OnInteractionEnd;
        }

        private void OnInteractionEnd(InteractionContext obj)
        {
            if (obj.Interactable != _currentRotatable) return;

            _targetingRay.Destroy();
            _targetingRay                            =  null;
            _rotationController.OnTargetAngleChanged -= UpdateTargetAngle;
        }

        private void OnRotationInteractionBegin(RotationInteractionContext context)
        {
            _currentRotatable = context.Interactable;

            _rotationController.OnTargetAngleChanged += UpdateTargetAngle;
            _targetingRay                            =  new();

            _targetAngle = _emitterRotatable.RotationHandler.CurrentAngle;

            UpdateTargetingRay();
        }

        private void UpdateTargetAngle(Angle angle)
        {
            if (_currentRotatable != _emitterRotatable) return;

            _targetAngle = angle;
        }

        private void FixedUpdate()
        {
            UpdateTargetingRay();
        }

        private void UpdateTargetingRay()
        {
            if (_targetingRay == null) return;

            float radius = _emitterRotatable.Radius;

            Vector3 direction = Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;

            Vector3 baseOrigin = _emitterRotatable.RotationTransform.position.With(y: transform.position.y);
            Vector3 rayOrigin  = baseOrigin + (direction * radius);

            debugPrint($"TargetAngle: {_targetAngle}, , Direction: {direction}, BaseOrigin: {baseOrigin}, RayOrigin: {rayOrigin}");

            _targetingRay.Origin    = rayOrigin;
            _targetingRay.Direction = direction;
        }
    }
}