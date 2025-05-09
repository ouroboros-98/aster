using System;
using System.Collections.Generic;
using System.Linq;
using Aster.Core.Interactions.Grab;
using Aster.Entity.Player;
using Aster.Utils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Core
{
    public class BaseGrabbable : AsterMono, IGrabbable, IInteractable
    {
        [SerializeField, Range(0, 2)]
        private float grabLiftHeight = 0.2f;

        [SerializeField, Range(0, 50)]
        private float grabFollowSpeed = 10f;

        public float GrabFollowSpeed => grabFollowSpeed;


        public GameObject GameObject         => gameObject;
        public Transform  GrabbableTransform => transform;

        [SerializeField]
        private Transform pedestalTransform;

        private float originalY;
        private bool  isGrabbed = false;

        private List<MonoBehaviour> disableOnGrab;
        

        private void Awake()
        {
            disableOnGrab = transform.GetComponentsInChildren<IDisableOnGrab>().Select(c => (MonoBehaviour)c).ToList();
        }

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
            disableOnGrab.ForEach(c => c.enabled = false);
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

            disableOnGrab.ForEach(c => c.enabled = true);
        }

        public void CanPlaceAt(Vector3 targetPosition)
        {
            
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