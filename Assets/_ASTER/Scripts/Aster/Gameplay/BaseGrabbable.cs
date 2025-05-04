using System;
using Aster.Entity.Player;
using Aster.Utils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Core
{
    public class BaseGrabbable : AsterMono, IGrabbable, IInteractable
    {
        [SerializeField, Range(0, 2)] private float grabLiftHeight = 0.2f;

        public GameObject GameObject         => gameObject;
        public Transform  GrabbableTransform => transform;

        [SerializeField] private Transform pedestalTransform;

        private float originalY;
        private bool  isGrabbed = false;

        private void Start()
        {
            originalY = transform.position.y;
        }

        public bool TryGrab()
        {
            return true;
        }

        public void OnGrab()
        {
        }

        public void DuringGrab()
        {
            transform.position = transform.position.With(y: originalY + grabLiftHeight);
            isGrabbed          = true;

            pedestalTransform = null;
            RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, 2f);
            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider.ScanForComponent(out CentralPedestal pedestal, parents: true)) continue;

                pedestalTransform = pedestal.transform;
            }
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
            isGrabbed          = false;
        }

        private void Update()
        {
            if (!isGrabbed && pedestalTransform != null)
            {
                if (transform.position.With(y: 0) != pedestalTransform.position.With(y: 0))
                {
                    transform.position = pedestalTransform.position.With(y: originalY);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.ScanForComponent(out CentralPedestal pedestal, parents: true)) return;
            pedestalTransform = pedestal.transform;
        }

        private void OnCollisionExit(Collision other)
        {
            if (!other.transform.ScanForComponent(out CentralPedestal pedestal, parents: true)) return;
            if (pedestalTransform == pedestal.transform)
            {
                pedestalTransform = null;
            }
        }
    }
}