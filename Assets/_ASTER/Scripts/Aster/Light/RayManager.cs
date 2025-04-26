using System;
using System.Collections.Generic;
using Aster.Core;
using Aster.Utils.Pool;

namespace Aster.Light
{
    public class RayManager : AsterMono
    {
        private IPool<LightRayObject> rayPool;

        private HashSet<ILightRay> _activeRays;


        private void Awake()
        {
            _activeRays = new HashSet<ILightRay>();
            rayPool     = RayPool.Instance;

            GameEvents.OnRayActivated += OnRayCreated;
        }

        private void OnDestroy()
        {
            GameEvents.OnRayActivated -= OnRayCreated;
        }

        private void OnRayCreated(ILightRay ray)
        {
            LightRayObject lightRayObject = rayPool.Get();

            lightRayObject.Data = ray;
            _activeRays.Add(ray);
        }

        private void FixedUpdate()
        {
            HashSet<ILightRay> raysToDestroy = new HashSet<ILightRay>();
            foreach (ILightRay ray in _activeRays)
            {
                if (ray.CheckExists()) continue;

                raysToDestroy.Add(ray);
            }

            foreach (ILightRay ray in raysToDestroy)
            {
                ray.Destroy();
                _activeRays.Remove(ray);
            }
        }
    }
}