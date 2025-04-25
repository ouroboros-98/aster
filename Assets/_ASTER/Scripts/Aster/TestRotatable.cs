using System;
using Aster.Entity.Player;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Core
{
    public class TestRotatable : AsterMono, IRotatatble, IInteractable
    {
        GameObject IInteractable.GameObject => gameObject;

        [SerializeField, BoxedProperty] private RotationHandler rotationHandler = new();
        [SerializeField]                private Transform       rotationTransform;
        [SerializeField]                private float           radius = 1f;

        public RotationHandler RotationHandler
        {
            get => rotationHandler;
        }

        public Transform RotationTransform
        {
            get => rotationTransform;
            set => rotationTransform = value;
        }

        public float Radius
        {
            get => radius;
            set => radius = value;
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
            ValidateComponent(ref rotationTransform);

            RotationHandler.Bind(rotationTransform);
        }

        public Action<PlayerController> Interact()
        {
            return (player) => GameEvents.OnRotationInteractionBegin?.Invoke(new(player, this));
        }
    }
}