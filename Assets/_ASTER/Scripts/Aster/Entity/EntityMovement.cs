using UnityEngine;

namespace _ASTER.Scripts.Aster.Entity
{
    [System.Serializable]
    public class EntityMovement
    {
        [SerializeField, Range(0, 50)] private float moveSpeed = 5;

        private ITargetMovementProvider _movementProvider;
        private Rigidbody               _rb;

        private bool IsInitialized => (_rb != null) && (_movementProvider != null);

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
            Vector3 targetVelocity  = targetDirection.normalized * (moveSpeed * Time.fixedDeltaTime);
            Vector3 targetPosition  = _rb.position + targetVelocity;

            _rb.MovePosition(targetPosition);
        }
    }
}