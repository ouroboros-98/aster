using System;
using Aster.Entity.Player;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Core
{
    public class BaseRotatable : AsterMono, IRotatatble, IInteractable
    {
        GameObject IInteractable.GameObject => gameObject;

        [SerializeField, BoxedProperty]
        private RotationHandler rotationHandler = new();

        [SerializeField]
        private Transform rotationTransform;

        [SerializeField]
        private float radius = 1f;

        public RotationHandler RotationHandler
        {
            get => rotationHandler;
            protected set => rotationHandler = value;
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

        protected virtual void Awake()
        {
            Reset();
        }

        protected virtual void Update()
        {
            RotationHandler.Update();
        }

        protected virtual void Reset()
        {
            ValidateComponent(ref rotationTransform);

            RotationHandler.Bind(rotationTransform);
        }

        public virtual Action<PlayerController> Interact()
        {
            return (player) =>
                       GameEvents.OnRotationInteractionBegin
                                ?.Invoke(new AnchorRotationInteractionContext(player, this));
        }
    }
}