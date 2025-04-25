using System;
using Aster.Core;
using UnityEngine;

namespace Aster.Light
{
    public abstract class BaseLightHittable : AsterMono, IDuplicatable
    {
        private            TargetingRayMarker _targetingRayMarker;
        public             bool               IsTargetingOnly => _targetingRayMarker != null;
        protected abstract LightHitContext    OnLightRayHit(LightHit lightHit);

        protected virtual void Awake()
        {
            Reset();
        }

        private void Reset()
        {
            _targetingRayMarker = GetComponentInParent<TargetingRayMarker>();
        }

        public LightHitContext LightHit(LightHit hit)
        {
            if (hit.Ray is not TargetingRay && IsTargetingOnly)
            {
                return new LightHitContext(hit, blockLight: false);
            }

            return OnLightRayHit(hit);
        }


        public virtual void OnLightRayExit(LightRayObject rayObject)
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