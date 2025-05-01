using System.Collections.Generic;

namespace Aster.Light
{
    public class RayHitTracker
    {
        private readonly ILightRay ray;

        private Dictionary<BaseLightHittable, LightHitContext> _rayHits = new();

        public RayHitTracker(ILightRay ray)
        {
            this.ray = ray;
        }

        public void RefreshHittables(HashSet<BaseLightHittable> hittables)
        {
            HashSet<BaseLightHittable> allHittables = new(_rayHits.Keys);

            HashSet<BaseLightHittable> toRemove = new(allHittables);
            toRemove.ExceptWith(hittables);

            foreach (var hittable in toRemove)
            {
                hittable.OnLightRayExit(ray);
                _rayHits.Remove(hittable);
            }
        }

        public bool HandleHit(LightHitContext hitContext)
        {
            _rayHits[hitContext.Hit.Hittable] = hitContext;

            return !hitContext.BlockLight;
        }
    }
}