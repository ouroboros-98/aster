using System.Collections.Generic;
using UnityEngine;

namespace Aster.Light
{
    public class LightRayProcessor
    {
        private readonly ILightCaster                         _lightCaster;
        private readonly Dictionary<ILightRay, RayHitTracker> _trackers;
        private readonly HashSet<ILightRay>                   _raysToRemove;

        public LightRayProcessor()
        {
            _raysToRemove = new();
            _trackers     = new();
            _lightCaster  = InitializeLightCaster();
        }

        protected virtual ILightCaster InitializeLightCaster()
        {
            return new LightSphereCaster();
        }

        public virtual void ValidateRay(ILightRay lightRay)
        {
            if (_trackers.ContainsKey(lightRay))
            {
                return;
            }

            _trackers[lightRay] = new RayHitTracker(lightRay);
        }

        public virtual void UnregisterRay(ILightRay lightRay)
        {
            if (!_trackers.ContainsKey(lightRay))
            {
                return;
            }

            _trackers[lightRay].RefreshHittables(new HashSet<BaseLightHittable>());
            _raysToRemove.Add(lightRay);
        }

        private void HandleRaysToRemove()
        {
            foreach (var ray in _raysToRemove)
            {
                _trackers.Remove(ray);
            }

            _raysToRemove.Clear();
        }

        public void ProcessRays(HashSet<ILightRay> rays)
        {
            rays = new(rays);
            HandleRaysToRemove();

            foreach (var ray in rays)
            {
                CastSingleRay(ray);
            }

            HandleRaysToRemove();
        }

        private void CastSingleRay(ILightRay ray)
        {
            if (!ray.CheckExists()) return;

            ValidateRay(ray);

            List<LightHit> hits = _lightCaster.GetHits(ray);

            HashSet<BaseLightHittable> hittables = new();

            if (hits.Count == 0)
            {
                // Debug.Log("Ray hit nothing");
                ray.EndPoint = ray.Origin + ray.Direction * ray.MaxDistance;
            }

            foreach (var hit in hits)
            {
                if (ray.CheckIgnoreHittable(hit.Hittable)) continue;

                var hitContext = hit.Hittable.LightHit(new(ray, hit.HitPoint, hit.Hittable));

                hittables.Add(hit.Hittable);

                if (!_trackers[ray].HandleHit(hitContext)) break;
            }

            _trackers[ray].RefreshHittables(hittables);
        }
    }
}