using System;
using Aster.Core;
using Aster.Utils.Pool;
using UnityEngine;

namespace Aster.Light
{
    [RequireComponent(typeof(LineRenderer))]
    public class LightRay : AsterMono, IPoolable
    {
        [SerializeField] private float maxDistance;
        private LightRay _creator;
        private Vector3 _direction;
        private Vector3 _origin;
        private Color _color;
        private bool isActive = true;
        [SerializeField] private float intensity;

        private LineRenderer _lineRenderer;

        public Vector3 GetDirection() => _direction;
        public Vector3 GetOrigin() => _origin;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.startWidth = 0.1f; // Adjust width as needed
            _lineRenderer.endWidth = 0.1f;
        }

        public void Initialize(Vector3 origin, Vector3 direction, Color color, float intensity)
        {
            _origin = origin;
            _direction = direction.normalized;
            _color = color;
            this.intensity = intensity;

            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }

        public void Reset()
        {
            isActive = false;
            _lineRenderer.enabled = false;
        }

        private void CheckForCreator()
        {
            if (_creator != null && !_creator.isActive)
                Destroy(gameObject);
        }

        private void Update()
        {
            if (!isActive) return;

            CheckForCreator();

            var distance = maxDistance;
            // Perform a raycast to detect objects in the path of the LightRay
            if (Physics.Raycast(_origin, _direction, out var hit, maxDistance))
            {
                distance = hit.distance; // Stop at the hit point
                HandleHit(hit.collider.gameObject);
            }

            // Update the LineRenderer to visually represent the LightRay
            _lineRenderer.SetPosition(0, _origin);
            _lineRenderer.SetPosition(1, _origin + _direction * distance);
        }

        private void HandleHit(GameObject hitObject)
        {
            if (hitObject.CompareTag("Enemy"))
            {
                Debug.Log($"Hit enemy: {hitObject.name}");
                // Add logic to handle enemy hit (e.g., damage)
            }
        }
    }
}