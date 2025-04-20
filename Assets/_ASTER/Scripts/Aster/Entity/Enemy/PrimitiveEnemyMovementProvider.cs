using Aster.Entity;
using UnityEngine;

namespace _ASTER.Scripts.Aster.Entity.Enemy
{
    public class PrimitiveEnemyMovementProvider : ITargetMovementProvider
    {
        private Transform _enemyTransform;
        private Transform _targetTransform;
        private float     _minDistance = 0.5f;

        public Vector2 TargetMovement => GetTargetMovement();

        public PrimitiveEnemyMovementProvider(Transform enemyTransform)
        {
            _enemyTransform = enemyTransform;
        }

        public void SetTarget(Transform target)
        {
            _targetTransform = target;
        }

        private Vector2 GetTargetMovement()
        {
            if (_targetTransform == null) return Vector2.zero;

            Vector2 targetPosition2D = new(_targetTransform.position.x, _targetTransform.position.z);
            Vector2 enemyPosition2D  = new(_enemyTransform.position.x, _enemyTransform.position.z);

            Vector2 direction = targetPosition2D - enemyPosition2D;

            float distance = direction.magnitude;

            // Debug.Log($"Enemy Position: {enemyPosition2D}, Target Position: {targetPosition2D}, Direction: {direction}, Distance: {distance}");
            if (distance <= _minDistance) return Vector2.zero;

            return direction.normalized;
        }
    }
}