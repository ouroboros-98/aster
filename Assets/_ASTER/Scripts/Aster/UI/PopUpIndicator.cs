using System;
using Aster.Core.Entity;
using Aster.Entity.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace Aster.Core.UI
{
    public class PopUpIndicator : AsterMono
    {
        private LookAtConstraint _lookAt;

        [ShowNonSerializedField] private Transform _cameraAimPoint;

        [SerializeField] private Image indicatorImage;
        
        [SerializeField] private Image[] indicatorImages;
        [SerializeField] private PlayerController player;

        private void Awake()
        {
            Reset();

            if (_cameraAimPoint == null) _cameraAimPoint = AsterCamera.Instance.AimPoint;

            _lookAt.AddSource(
                new ConstraintSource
                {
                    sourceTransform = _cameraAimPoint,
                    weight          = 1
                }
            );
        }

        private void OnEnable()
        {
            AsterEvents.Instance.OnGrabInteractionBegin += DisableLeft;
            AsterEvents.Instance.OnRotationInteractionBegin+= DisableLeft;
            AsterEvents.Instance.OnRotationInteractionBegin += DisableUp;
            AsterEvents.Instance.OnInteractionEnd+= EnableLeft;
            AsterEvents.Instance.OnInteractionEnd+= EnableUp;
        }
        private void OnDisable()
        {
            AsterEvents.Instance.OnGrabInteractionBegin -= DisableLeft;
            AsterEvents.Instance.OnInteractionEnd -= EnableLeft;
            AsterEvents.Instance.OnInteractionEnd -= EnableUp;
        }
        public void SetEnabled(bool enabled)
        {
            indicatorImage.gameObject.SetActive(enabled);
        }
        

        private void Reset()
        {
            ValidateComponent(ref _lookAt);
        }

        private void DisableUp(InteractionContext context)
        {
            if(context.Player==player)
                indicatorImages[3].gameObject.SetActive(false);
        }

        private void EnableUp(InteractionContext context)
        {
            if(context.Player==player)
                indicatorImages[3].gameObject.SetActive(true);
        }
        private void DisableLeft(InteractionContext context)
        {
            if(context.Player==player)
                indicatorImages[0].gameObject.SetActive(false);
        }

        private void EnableLeft(InteractionContext context)
        {
            if(context.Player==player)
                indicatorImages[0].gameObject.SetActive(true);
        }
        
            
        
        private void DisableDown(InteractionContext context)
        {
            if(context.Player==player)
                indicatorImages[1].gameObject.SetActive(false);
        }

        private void EnableDown(InteractionContext context)
        {
            if(context.Player==player)
                indicatorImages[1].gameObject.SetActive(true);
        }
        private void DisableRight(InteractionContext context)
        {
           if(context.Player==player)
               indicatorImages[2].gameObject.SetActive(false);
        }

        private void EnableRight(InteractionContext context)
        {
            if(context.Player==player)
                indicatorImages[2].gameObject.SetActive(true);
        }

    }
}