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
            rayPool = RayPool.Instance;

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
        }
    }
}