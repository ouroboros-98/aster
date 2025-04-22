using System;
using System.Collections.Generic;
using Aster.Core;
using Aster.Utils.Pool;

namespace Aster.Light
{
    public class RayManager : AsterMono
    {
        private IPool<LightRay> rayPool;

        private HashSet<RayData> _activeRays;


        private void Awake()
        {
            rayPool = RayPool.Instance;

            GameEvents.OnRayCreated += OnRayCreated;
        }

        private void OnDestroy()
        {
            GameEvents.OnRayCreated -= OnRayCreated;
        }

        private void OnRayCreated(RayData ray)
        {
            LightRay lightRay = rayPool.Get();

            lightRay.Data = ray;
        }
    }
}