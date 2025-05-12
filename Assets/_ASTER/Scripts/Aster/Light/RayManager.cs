using System;
using System.Collections.Generic;
using Aster.Core;
using Aster.Utils.Pool;
using UnityEngine;

namespace Aster.Light
{
    public class RayManager : AsterMono
    {
        [SerializeField]
        [Range(0, 2000)]
        private int maxRayCount = 100;

        private IPool<LightRayObject> rayPool;

        private HashSet<ILightRay> _activeRays;
        private LightRayProcessor  _rayProcessor;

        private void Awake()
        {
            _activeRays   = new HashSet<ILightRay>();
            _rayProcessor = new LightRayProcessor();
            rayPool       = RayPool.Instance;

            GameEvents.OnRayActivated += OnRayCreated;
        }

        private void OnDestroy()
        {
            GameEvents.OnRayActivated -= OnRayCreated;
        }

        private void OnRayCreated(ILightRay ray)
        {
            if (_activeRays.Count >= maxRayCount)
            {
                ray.Destroy();
                return;
            }

            LightRayObject lightRayObject = rayPool.Get();

            lightRayObject.Data = ray;
            _activeRays.Add(ray);
            _rayProcessor.ValidateRay(ray);

            ray.OnDestroy += () => _rayProcessor.UnregisterRay(ray);
        }

        private void FixedUpdate()
        {
            _rayProcessor?.ProcessRays(_activeRays);

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