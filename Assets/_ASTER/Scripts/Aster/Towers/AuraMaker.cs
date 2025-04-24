using System;
using System.Collections.Generic;
using Aster.Light;
using Aster.Utils.Pool;
using UnityEngine;

namespace Aster.Towers
{
    public class AuraMaker : BaseTower
    {
        [SerializeField] private float radius = 5f;
        private Dictionary<LightRay, LightRay> reflections = new();

        private Vector3 MirrorNormal => transform.forward;

        public override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            UpdateReflection(lightHit.Ray, lightHit.HitPoint);

            return new(lightHit, blockLight: true);
        }

        private Vector3 GetReflectionDir(LightRay ray) => Vector3.Reflect(ray.GetDirection(), MirrorNormal);

        private void CreateLightReflection(LightRay rayIn)
        {
            Vector3 newLightDirection = GetReflectionDir(rayIn);

            Vector3 hitPosition = rayIn.GetHitPosition();
            // Instantiate the LightRay prefab
            var reflectionRay = RayPool.Instance.Get();

            // Initialize the LightRay
            reflectionRay.Initialize(hitPosition, newLightDirection, Color.yellow, 1, rayIn);

            reflections[rayIn] = reflectionRay;
        }

        public void Update()
        {
            // List<LightRay> raysIn = new List<LightRay>(reflections.Keys);
            //
            // List<LightRay> raysToRemove = new();
            //
            // foreach (LightRay rayIn in raysIn)
            // {
            //     UpdateReflection(rayIn);
            // }
            //
            // foreach (LightRay rayIn in raysToRemove)
            // {
            //     reflections.Remove(rayIn);
            // }
        }

        public override void OnLightRayExit(LightRay ray)
        {
            LightRay rayOut = reflections[ray];
            if (rayOut != null) RayPool.Instance.Return(rayOut);

            reflections.Remove(ray);
        }

        private void UpdateReflection(LightRay rayIn, Vector3 hitPoint)
        {
            if (!ValidateReflection(rayIn))
            {
                CreateLightReflection(rayIn);
                return;
            }

            LightRay rayOut       = reflections[rayIn];
            Vector3  newDirection = GetReflectionDir(rayIn);

            rayOut.Direction = newDirection.normalized;
            rayOut.Origin    = hitPoint;
        }

        private bool ValidateReflection(LightRay rayIn)
        {
            if (!reflections.ContainsKey(rayIn)) return false;
            LightRay ray = reflections[rayIn];
            return ray != null && ray.gameObject.activeSelf && ray.enabled;
        }
    }
}