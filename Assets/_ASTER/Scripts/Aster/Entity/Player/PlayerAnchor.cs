using System;
using Aster.Core;
using Aster.Utils;
using DependencyInjection;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class PlayerAnchor : AsterMono
    {
        [ShowNonSerializedField, ReadOnly] private IRotatatble        _anchor;
        [Inject]                           private RotationController _rotationController;

        private float _targetAngle;

        public IRotatatble Anchor => _anchor;

        public void HandleAnchoring()
        {
            if (_anchor == null) return;

            Vector3 basePosition = _anchor.RotationTransform.position.With(y: transform.position.y);
            Vector3 direction    = Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;

            transform.position = basePosition + (direction * _anchor.Radius);
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        private void OnEnable()
        {
            GameEvents.OnRotationInteractionBegin += HandleRotationInteractionBegin;
            GameEvents.OnInteractionEnd           += HandleInteractionEnd;
        }

        private void HandleInteractionEnd(InteractionContext obj)
        {
            if (obj.Interactable != _anchor) return;
            _anchor                                  =  null;
            _rotationController.OnTargetAngleChanged -= UpdateTargetAngle;
        }

        private void OnDisable()
        {
            GameEvents.OnRotationInteractionBegin -= HandleRotationInteractionBegin;
        }

        public void UpdateTargetAngle(Angle angle) => _targetAngle = angle;

        public bool IsAnchoring() => _anchor != null;

        private void HandleRotationInteractionBegin(RotationInteractionContext context)
        {
            if (!context.Anchor) return;

            _anchor      = context.Interactable;
            _targetAngle = _rotationController.TargetAngle;

            _rotationController.OnTargetAngleChanged += UpdateTargetAngle;

            Debug.Log($"Anchor Start", _anchor.RotationTransform.gameObject);
        }
    }
}