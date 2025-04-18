using System;
using Aster.Entity.Player;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Core
{
    public class TestRotatable : AsterMono, IRotatatble
    {
        [SerializeField, BoxedProperty] private RotationHandler rotationHandler = new();

        public RotationHandler RotationHandler   => rotationHandler;
        public Transform       RotationTransform => transform;

        public float TargetAngle = 0f;

        [Button("EditorRotate")]
        private void EditorRotate()
        {
            RotationHandler.Rotate(TargetAngle);
        }

        private void Awake()
        {
            Reset();
        }

        private void Update()
        {
            RotationHandler.Update();
        }

        public void Reset()
        {
            RotationHandler.Bind(transform);
        }

        public Action<PlayerController> Interact()
        {
            return (player) => GameEvents.OnRotationInteractionBegin?.Invoke(new(player, this));
        }
    }
}