using System;
using Aster.Core;
using Aster.Towers;
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
        private bool isUsed=false;
        [SerializeField] private float intensity;
        private GameObject _hitMirror; // Store the mirror this ray hit
        private Vector3 _hitPosition;

        private LineRenderer _lineRenderer;

        public Vector3 GetDirection() => _direction;
        public Vector3 GetOrigin() => _origin;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.startWidth = 0.05f; // Adjust width as needed
            _lineRenderer.endWidth = 0.05f;
        }

        public void Initialize(Vector3 origin, Vector3 direction, Color color, float intensity, LightRay creator=null)
        {
            _origin = origin;
            _direction = direction.normalized;
            _color = color;
            this.intensity = intensity;
            _creator=creator;
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }

        public void Reset()
        {
            isActive = false;
            isUsed = false;
            _lineRenderer.enabled = false;
        }

        private void CheckForCreator()
        {
            if (_creator != null && (!_creator.isActive|| !_creator.isUsed))
                Destroy(gameObject);
        }

        private void Update()
        {
            CheckForCreator();
            if (!isActive) return;
            var distance = maxDistance;
            var ray= Physics.Raycast(_origin, _direction, out var hit, maxDistance);
            // Perform a raycast to detect objects in the path of the LightRay
            if (ray)
            {
                distance = hit.distance; // Stop at the hit point
                _hitPosition= hit.point;
                HandleHit(hit.collider.gameObject);
            }
            else
            {
                isUsed=false;
                _hitMirror=null;
            }

            // Update the LineRenderer to visually represent the LightRay
            _lineRenderer.SetPosition(0, _origin);
            _lineRenderer.SetPosition(1, _origin + _direction * distance);
        }

        private void HandleHit(GameObject hitObject)
        {
            if (hitObject.CompareTag("Enemy"))
            {
                isActive = false;
            }
            else if (hitObject.CompareTag("Mirror") && !isUsed)
            {
                // Ensure the ray only interacts with the mirror it directly hit
                if (_hitMirror == null)
                {
                    _hitMirror = hitObject; // Set the mirror this ray hit
                    isUsed = true;
                    var mirror = hitObject.GetComponent<BaseTower>();
                    mirror.OnLightRayHit(this);
                }
               
            }
        }

        public Vector3 GetHitPosition()
        {
            return _hitPosition;
        }
    }
}