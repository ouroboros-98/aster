using System;
using Aster.Core;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Light
{
    public abstract class BaseLightHittable : AsterMono, IDuplicatable
    {
        private TargetingRayMarker _targetingRayMarker;

        [ShowNonSerializedField] private bool _isTargetingOnly;

        public bool IsTargetingOnly => _isTargetingOnly;

        protected abstract LightHitContext OnLightRayHit(LightHit lightHit);

        protected virtual void Awake()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            _targetingRayMarker = GetComponentInParent<TargetingRayMarker>();
            _isTargetingOnly    = _targetingRayMarker != null;
        }

        public LightHitContext LightHit(LightHit hit)
        {
            bool isTargetingRay = hit.Ray is TargetingRay;

            if (!isTargetingRay && _isTargetingOnly)
            {
                return new LightHitContext(hit, blockLight: false);
            }

            return OnLightRayHit(hit);
        }


        public virtual void OnLightRayExit(ILightRay ray)
        {
            // Optional: Implement if needed
        }

        public void OnDuplicate()
        {
            Reset();
        }
    }

    public readonly struct LightHit
    {
        public readonly ILightRay         Ray;
        public readonly Vector3           HitPoint;
        public readonly BaseLightHittable Hittable;

        public readonly float Distance => Vector3.Distance(Ray.Origin, HitPoint);

        public LightHit(ILightRay ray, Vector3 hitPoint, BaseLightHittable hittable)
        {
            Ray      = ray;
            HitPoint = hitPoint;
            Hittable = hittable;
        }
    }

    public readonly struct LightHitContext
    {
        public readonly LightHit Hit;
        public readonly bool     BlockLight;

        public LightHitContext(LightHit hit, bool blockLight = false, float blockDistance = 0f)
        {
            Hit        = hit;
            BlockLight = blockLight;

            if (blockLight) HandleBlocking(blockDistance);
        }

        private void HandleBlocking(float distance)
        {
            Vector3 offset = -distance * Hit.Ray.Direction;
            Hit.Ray.EndPoint = Hit.HitPoint + offset;
        }
    }
}