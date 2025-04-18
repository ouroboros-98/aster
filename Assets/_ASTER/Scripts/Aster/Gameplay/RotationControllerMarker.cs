using Aster.Utils;
using UnityEngine;

namespace Aster.Core
{
    public class RotationControllerMarker : AsterMono
    {
        [SerializeField] private RotationController rotationController;
        [SerializeField] private Transform          markerSpriteTransform;
        [SerializeField] private Transform          targetTransform;

        private void Awake()
        {
            Reset();
            Hide();
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

            OnTargetAngleChanged(context.Interactable.RotationHandler.CurrentAngle);
        }

        private void Reset()
        {
            ValidateComponent(ref rotationController, self: false, parents: true);
        }
    }
}