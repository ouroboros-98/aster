using Aster.Core;
using UnityEngine;

namespace Aster.Light
{
    public abstract class BaseLightHittable : AsterMono
    {
        public abstract LightHitContext OnLightRayHit(LightHit lightHit);

        public virtual void OnLightRayExit(LightRay ray)
        {
            // Optional: Implement if needed
        }
    }

    public readonly struct LightHit
    {
        public readonly LightRay          Ray;
        public readonly Vector3           HitPoint;
        public readonly BaseLightHittable Hittable;
        public readonly Vector3           RayDirection;

        public LightHit(LightRay ray, Vector3 hitPoint, BaseLightHittable hittable, Vector3 rayDirection)
        {
            Ray               = ray;
            HitPoint          = hitPoint;
            Hittable          = hittable;
            RayDirection = rayDirection;
        }
    }

    public readonly struct LightHitContext
    {
        public readonly LightHit Hit;
        public readonly bool     BlockLight;

        public LightHitContext(LightHit hit, bool blockLight = false)
        {
            Hit        = hit;
            BlockLight = blockLight;
        }
    }
}