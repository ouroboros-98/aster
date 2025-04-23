using UnityEngine;

namespace Aster.Entity
{
    [System.Serializable]
    public class EntityMovement
    {
        private const float THRESHOLD = 0.2f;

        [SerializeField, Range(0, 50)] private float moveSpeed = 5;

        private ITargetMovementProvider _movementProvider;
        private Rigidbody               _rb;
        private bool                    IsInitialized => (_rb != null) && (_movementProvider != null);

        public void Init(Rigidbody rb, ITargetMovementProvider movementProvider)
        {
            _rb               = rb;
            _movementProvider = movementProvider;
        }

        public void HandleMovement()
        {
            if (!IsInitialized)
            {
                Debug.LogError("EntityMovement is not initialized. Please call Init() before using HandleMovement.");
                return;
            }

            float targetX = _movementProvider.TargetMovement.x;
            float targetZ = _movementProvider.TargetMovement.y;

            Vector3 targetDirection = new(targetX, 0, targetZ);
            if (targetDirection.magnitude < THRESHOLD)
            {
                targetDirection     = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }

            Vector3 targetVelocity = targetDirection.normalized * (moveSpeed * Time.fixedDeltaTime);
            Vector3 targetPosition = _rb.position + targetVelocity;

            _rb.MovePosition(targetPosition);
            if (targetDirection != Vector3.zero) _rb.MoveRotation(Quaternion.LookRotation(targetDirection));
        }
    }
}