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

        [SerializeField] private Image[]          indicatorImages;
        [SerializeField] private PlayerController player;

        private RotationInteractionContext _context;

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
            AsterEvents.Instance.OnGrabInteractionBegin     += DisableDown;
            AsterEvents.Instance.OnRotationInteractionBegin += OnRotationInteractionBegin;
            AsterEvents.Instance.OnRotationInteractionBegin += DisableDown;
            AsterEvents.Instance.OnInteractionEnd           += EnableDown;
            AsterEvents.Instance.OnInteractionEnd           += EnableRight;
            AsterEvents.Instance.OnInteractionEnd           += OnInteractionEnded;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnGrabInteractionBegin     -= DisableDown;
            AsterEvents.Instance.OnRotationInteractionBegin -= OnRotationInteractionBegin;
            AsterEvents.Instance.OnInteractionEnd           -= EnableDown;
            AsterEvents.Instance.OnInteractionEnd           -= EnableRight;
            AsterEvents.Instance.OnInteractionEnd           -= OnInteractionEnded;
        }

        void OnRotationInteractionBegin(RotationInteractionContext context)
        {
            if (context.Player != player) return;
            _context = context;
        }

        void OnInteractionEnded(InteractionContext context)
        {
            if (context != _context) return;
            _context = null;
            Show();
        }

        void Hide(InteractionContext context = null)
        {
            if (context == null || context.Player == player) SetEnabled(false);
        }

        void Show(InteractionContext context = null)
        {
            if (context == null || context.Player == player) SetEnabled(true);
        }

        public void SetEnabled(bool enabled)
        {
            indicatorImage.gameObject.SetActive(enabled);
        }

        private void Update()
        {
            if (_context != null) Hide();
        }

        private void Reset()
        {
            ValidateComponent(ref _lookAt);
        }

        private void DisableUp(InteractionContext context)
        {
            if (context.Player == player)
                indicatorImages[3].gameObject.SetActive(false);
        }

        private void EnableUp(InteractionContext context)
        {
            if (context.Player == player)
                indicatorImages[3].gameObject.SetActive(true);
        }

        private void DisableLeft(InteractionContext context)
        {
            if (context.Player == player)
                indicatorImages[0].gameObject.SetActive(false);
        }

        private void EnableLeft(InteractionContext context)
        {
            if (context.Player == player)
                indicatorImages[0].gameObject.SetActive(true);
        }


        private void DisableDown(InteractionContext context)
        {
            if (context.Player == player)
                indicatorImages[1].gameObject.SetActive(false);
        }

        private void EnableDown(InteractionContext context)
        {
            if (context.Player == player)
                indicatorImages[1].gameObject.SetActive(true);
        }

        private void DisableRight(InteractionContext context)
        {
            if (context.Player == player)
                indicatorImages[2].gameObject.SetActive(false);
        }

        private void EnableRight(InteractionContext context)
        {
            if (context.Player == player)
                indicatorImages[2].gameObject.SetActive(true);
        }
    }
}