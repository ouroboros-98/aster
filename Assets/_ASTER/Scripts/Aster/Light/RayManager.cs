using System;
using System.Collections.Generic;
using Aster.Core;
using Aster.Utils.Pool;

namespace Aster.Light
{
    public class RayManager : AsterMono
    {
        private IPool<LightRayObject> rayPool;

        private HashSet<LightRay> _activeRays;


        private void Awake()
        {
            rayPool = RayPool.Instance;

            GameEvents.OnRayCreated += OnRayCreated;
        }

        private void OnDestroy()
        {
            GameEvents.OnRayCreated -= OnRayCreated;
        }

        private void OnRayCreated(LightRay ray)
        {
            LightRayObject lightRayObject = rayPool.Get();

            lightRayObject.Data = ray;
        }
    }
}