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
        [SerializeField]                private Transform       rotationTransform;
        [SerializeField]                private float           radius = 1f;

        public RotationHandler RotationHandler   => rotationHandler;
        public Transform       RotationTransform => rotationTransform;
        public float           Radius            => radius;

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
            RotationHandler.Bind(rotationTransform);
        }

        public Action<PlayerController> Interact()
        {
            return (player) => GameEvents.OnRotationInteractionBegin?.Invoke(new(player, this));
        }
    }
}