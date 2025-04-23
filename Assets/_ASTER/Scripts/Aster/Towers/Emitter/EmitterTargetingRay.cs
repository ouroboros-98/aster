using System;
using Aster.Core;
using Aster.Light;
using DependencyInjection;
using UnityEngine;

namespace Aster.Towers
{
    public class EmitterTargetingRay : AsterMono
    {
        [Inject] private RotationController _rotationController;
        [Inject] private InputHandler       _inputHandler;

        private IRotatatble  _rotatable;
        private TargetingRay _targetingRay;

        private bool IsTargetingRayActive => _targetingRay != null && _targetingRay.IsActive;

        private void Awake()
        {
            _rotatable    = GetComponent<IRotatatble>();
            _targetingRay = null;
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
            if (obj.Interactable != _rotatable) return;

            _rotatable = null;
            _targetingRay.Destroy();
        }

        private void OnRotationInteractionBegin(RotationInteractionContext context)
        {
            if (context.Interactable != _rotatable) return;

            _targetingRay = new();
        }

        private void FixedUpdate()
        {
            if (!IsTargetingRayActive) return;

            UpdateTargetingRay();
        }

        public void UpdateTargetingRay()
        {
            if (_targetingRay == null) return;

            float radius = _rotatable.Radius;

            Vector2 direction2D = _inputHandler.Rotation;
            Vector3 direction3D = new Vector3(direction2D.x, 0, direction2D.y);

            _targetingRay.Origin    = _rotatable.RotationTransform.position + (direction3D * radius);
            _targetingRay.Direction = direction3D;
        }
    }
}