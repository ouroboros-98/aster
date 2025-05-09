using System;
using Aster.Entity.Player;
using Aster.Utils;
using QFSW.QC;
using UnityEngine;

namespace Aster.Core.FX
{
    public class PlayerGrabIndicator : AsterMono
    {
        private const int POINT_PLAYER       = 0;
        private const int POINT_INTERACTABLE = 1;

        [SerializeField]
        [ColorUsage(true, true)]
        private Color canPlaceLineColor;

        [SerializeField]
        [ColorUsage(true, true)]
        private Color cannotPlaceLineColor;

        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private PlayerController player;

        [SerializeField, Range(-1, 1)]
        private float zOffset = 0f;


        private GrabInteractionContext currentContext;

        private Transform grabbableTransform;

        private void Awake()
        {
            Hide();
        }

        private void OnEnable()
        {
            player.Grabber.OnCanPlaceChanged  += OnCanPlaceChanged;
            GameEvents.OnGrabInteractionBegin += OnGrabInteractionBegin;
            GameEvents.OnInteractionEnd       += OnGrabInteractionEnd;
        }

        private void OnCanPlaceChanged(bool canPlace)
        {
            Color color = canPlace ? canPlaceLineColor : cannotPlaceLineColor;

            lineRenderer.endColor = lineRenderer.startColor = color;
        }

        private void OnDisable()
        {
            GameEvents.OnGrabInteractionBegin -= OnGrabInteractionBegin;
            GameEvents.OnInteractionEnd       -= OnGrabInteractionEnd;
        }

        private void OnGrabInteractionEnd(InteractionContext context)
        {
            if (currentContext != context) return;

            Hide();

            grabbableTransform = null;
            currentContext     = null;
        }

        private void Show() => lineRenderer.enabled = true;
        private void Hide() => lineRenderer.enabled = false;

        private void OnGrabInteractionBegin(GrabInteractionContext context)
        {
            if (context.Player != player) return;

            currentContext = context;

            grabbableTransform = context.Interactable.GrabbableTransform;
            if (grabbableTransform.ScanForComponent(out GrabIndicatorPoint point, children: true))
            {
                grabbableTransform = point.transform;
            }

            UpdatePlayerPoint();
            UpdateInteractablePoint();

            Show();
        }

        private void Update()
        {
            transform.rotation = Quaternion.identity;

            UpdatePlayerPoint();
            UpdateInteractablePoint();
        }

        private Vector3 UpdatePlayerPoint()
        {
            if (currentContext == null) return Vector3.zero;

            Vector3 playerPosition = Vector3.zero;
            playerPosition.z += zOffset;

            lineRenderer.SetPosition(POINT_PLAYER, playerPosition);

            return playerPosition;
        }

        private Vector3 UpdateInteractablePoint()
        {
            if (currentContext == null) return Vector3.zero;

            Vector3 interactableDistance = grabbableTransform.position - transform.position;
            // interactableDistance.z += zOffset;

            lineRenderer.SetPosition(POINT_INTERACTABLE, interactableDistance);
            return interactableDistance;
        }

        private void OnValidate()
        {
            ValidateComponent(ref player,       parents: true);
            ValidateComponent(ref lineRenderer, parents: true);
        }

        [Command("log-grab-indicator", MonoTargetType.Single)]
        void Log()
        {
            print($"{UpdatePlayerPoint().ToString()}, {UpdateInteractablePoint().ToString()}");
        }
    }
}