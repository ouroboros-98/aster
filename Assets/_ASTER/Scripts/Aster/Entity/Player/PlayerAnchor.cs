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
        [ShowNonSerializedField, ReadOnly] private IRotatatble _anchor;

        [SerializeField] private PlayerController         player;
        [SerializeField] private PlayerRotationController rotationController;

        private float _targetAngle;

        public IRotatatble Anchor => _anchor;

        private void Awake()
        {
            ValidateComponent(ref player, parents: true);
        }

        private void Start()
        {
            rotationController = player.RotationController;
        }

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
            if (obj.Player != player || obj.Interactable != _anchor) return;
            _anchor                                 =  null;
            rotationController.OnTargetAngleChanged -= UpdateTargetAngle;
        }

        private void OnDisable()
        {
            GameEvents.OnRotationInteractionBegin -= HandleRotationInteractionBegin;
            GameEvents.OnInteractionEnd           -= HandleInteractionEnd;
        }

        private void Update()
        {
            if (_anchor != null && !Config.Entities.PlayerRotateWithTowers)
            {
                _anchor                                 =  null;
                rotationController.OnTargetAngleChanged -= UpdateTargetAngle;
            }
        }

        public void UpdateTargetAngle(Angle angle) => _targetAngle = angle;

        public bool IsAnchoring() => _anchor != null;

        private void HandleRotationInteractionBegin(RotationInteractionContext context)
        {
            if (context.Player != player) return;
            if (!Config.Entities.PlayerRotateWithTowers || !context.Anchor) return;

            _anchor      = context.Interactable;
            _targetAngle = rotationController.TargetAngle;

            rotationController.OnTargetAngleChanged += UpdateTargetAngle;

            Debug.Log($"Anchor Start", _anchor.RotationTransform.gameObject);
        }
    }
}