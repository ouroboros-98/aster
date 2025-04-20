using System;
using Aster.Core;
using Aster.Towers;
using Aster.Utils.Pool;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Light
{
    [RequireComponent(typeof(LineRenderer))]
    public class LightRay : AsterMono, IPoolable
    {
        [SerializeField] private float    maxDistance;
        private                  LightRay _creator;
        private                  Color    _color;
        private                  bool     isActive = true;
        [SerializeField] private float    intensity;

        private Vector3 _origin;
        private Vector3 _endPoint;

        public Vector3 Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                _lineRenderer?.SetPosition(0, value);
            }
        }

        public Vector3 EndPoint
        {
            get => _endPoint;
            set
            {
                _endPoint = value;
                _lineRenderer?.SetPosition(1, value);
            }
        }

        public Vector3 Direction
        {
            get => (_endPoint       - Origin).normalized;
            set { EndPoint = Origin + value.normalized * maxDistance; }
        }

        private BaseLightHittable _hittable;

        private BaseLightHittable Hittable
        {
            get => _hittable;
            set
            {
                if (_hittable != null && _hittable == value) return;
                _hittable?.OnLightRayExit(this);
                _hittable = value;
            }
        }

        private LineRenderer _lineRenderer;
        private Vector3      _creatorInitialPosition;
        private Vector3      _creatorInitialDirection;

        public Vector3 GetDirection() => Direction;
        public Vector3 GetOrigin()    => Origin;

        private void Awake()
        {
            _lineRenderer            = GetComponent<LineRenderer>();
            _lineRenderer.startWidth = 0.05f; // Adjust width as needed
            _lineRenderer.endWidth   = 0.05f;
        }

        public void Initialize(Vector3 origin, Vector3 direction, Color color, float intensity, LightRay creator = null)
        {
            this.Origin              = origin;
            this.Direction           = direction.normalized;
            _color                   = color;
            this.intensity           = intensity;
            _creator                 = creator;
            Hittable                 = null;
            _lineRenderer.enabled    = true;
            _lineRenderer.startColor = color;
            _lineRenderer.endColor   = color;
            if (_creator != null)
            {
                _creatorInitialPosition  = _creator.GetOrigin();
                _creatorInitialDirection = _creator.GetDirection();
            }
        }

        public void Reset()
        {
            // isActive = false;
            Hittable?.OnLightRayExit(this);

            _lineRenderer.enabled = false;
            _creator              = null;
            Hittable              = null;
        }

        private void CheckForCreator()
        {
            if (_creator != null && (!_creator.isActive) || _creator != null &&
                (_creator.GetOrigin()    != _creatorInitialPosition ||
                 _creator.GetDirection() != _creatorInitialDirection))
                RayPool.Instance.Return(this);
        }

        private void FixedUpdate()
        {
            CheckForCreator();

            if (!isActive) return;

            bool hasHit = false;

            EndPoint = Origin + Direction * maxDistance;

            RaycastHit[] hits = Physics.RaycastAll(Origin, Direction, maxDistance);

            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider.TryGetComponent(out BaseLightHittable hittable)) continue;

                hasHit = true;
                var hitContext = hittable.OnLightRayHit(new(this, hit.point, hittable));
                Hittable = hittable;

                if (hitContext.BlockLight)
                {
                    EndPoint = hit.point;
                }
            }

            if (!hasHit) Hittable = null;
        }

        public Vector3 GetHitPosition() => EndPoint;
    }
}