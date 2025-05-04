using System;
using Aster.Entity.Player;
using Aster.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Core
{
    public class BaseGrabbable : AsterMono, IGrabbable, IInteractable
    {
        [SerializeField, Range(0, 2)] private float     grabLiftHeight = 0.3f;

        public GameObject GameObject         => gameObject;
        public Transform  GrabbableTransform => transform;

        private float originalY;

        private void Start()
        {
            originalY = transform.position.y;
        }

        public void OnGrab()
        {
        }

        public void DuringGrab()
        {
            transform.position = transform.position.With(y: originalY + grabLiftHeight);
        }

        public virtual Action<PlayerController> Interact()
        {
            return (player) =>
                       GameEvents.OnGrabInteractionBegin
                                ?.Invoke(new GrabInteractionContext(player, this));
        }

        public void OnRelease()
        {
            transform.position = transform.position.With(y: originalY);
        }
    }
}