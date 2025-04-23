using System;
using Aster.Core;
using UnityEngine;

namespace Aster.Light
{
    public abstract class BaseLightHittable : AsterMono
    {
        public abstract LightHitContext OnLightRayHit(LightHit lightHit);

        public virtual void OnLightRayExit(LightRayObject rayObject)
        {
            // Optional: Implement if needed
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