using System;
using Aster.Utils;
using DependencyInjection;
using UnityEngine;

namespace Aster.Core
{
    public class RotationControllerMarker : AsterMono
    {
        [SerializeField] private PlayerRotationController rotationController;

        [SerializeField] private Transform markerSpriteTransform;
        [SerializeField] private Transform targetTransform;

        private void Awake()
        {
            ValidateComponent(ref rotationController, parents: true);
        }

        private void OnEnable()
        {
            rotationController.OnInteractionBegin   += OnInteractionBegin;
            rotationController.OnTargetAngleChanged += OnTargetAngleChanged;
            rotationController.OnDeactivate         += OnDeactivate;
        }

        private void OnDisable()
        {
            rotationController.OnInteractionBegin   -= OnInteractionBegin;
            rotationController.OnTargetAngleChanged -= OnTargetAngleChanged;
            rotationController.OnDeactivate         -= OnDeactivate;
        }

        private void Start()
        {
            Hide();
        }

        private void OnDeactivate()
        {
            Hide();
            targetTransform = null;
        }

        private void OnTargetAngleChanged(Angle angle)
        {
            if (targetTransform == null) return;

            transform.position = targetTransform.position.With(y: transform.position.y);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        private void Show() => markerSpriteTransform?.gameObject.SetActive(true);
        private void Hide() => markerSpriteTransform?.gameObject.SetActive(false);


        private void OnInteractionBegin(RotationInteractionContext context)
        {
            Show();

            targetTransform = context.Interactable.RotationTransform;

            transform.localScale = Vector3.one * context.Interactable.Radius;

            OnTargetAngleChanged(context.Interactable.RotationHandler.CurrentAngle);
        }
    }
}