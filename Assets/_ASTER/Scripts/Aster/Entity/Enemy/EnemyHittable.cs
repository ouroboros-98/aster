using System;
using Aster.Core;
using Aster.Core.FX;
using Aster.Light;
using Aster.Towers;
using Aster.Utils;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Aster.Entity.Enemy
{
    public class EnemyHittable : BaseLightHittable
    {
        [SerializeField]
        private EnemyController enemyController;

        [SerializeField]
        private EnemyHitFXController hitFX;

        private LightReceiver        lightReceiver;
        private EnemyRayManipulation enemyRayManipulation = new();
        private RayManipulator       enemyRayManipulator;

        public bool IsPooled = false;

        private void Awake()
        {
            Reset();
        }

        private void Reset()
        {
            ValidateComponent(ref enemyController);
            ValidateComponent(ref hitFX, children: true);

            lightReceiver       = new();
            enemyRayManipulator = new(lightReceiver, transform, enemyRayManipulation);
        }

        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            if (lightHit.Ray is not TargetingRay)
            {
                enemyController.LightHit(lightHit);
            }

            lightHit.Ray.EndPoint = lightHit.HitPoint;

            lightReceiver.Register(lightHit);

            return new(lightHit, blockLight: true);
        }

        public override void OnLightRayExit(ILightRay ray)
        {
            lightReceiver.Deregister(ray);
        }

        private void Update()
        {
            if (lightReceiver.Count == 0)
                hitFX.Hide();
            else
                hitFX.Show();
        }

        void FixedUpdate()
        {
        }
    }

    [System.Serializable]
    public class EnemyRayManipulation : RayManipulation
    {
        [SerializeField]
        private float intensityScale = .8f;

        public override void Apply(LightHit hit, ILightRay rayIn, ILightRay rayOut)
        {
            rayOut.Origin      = hit.HitPoint;
            rayOut.MaxDistance = rayIn.MaxDistance - rayIn.Distance();
            rayOut.Direction   = rayIn.Direction;
            rayOut.Intensity   = rayIn.Intensity * intensityScale;

            rayOut.IgnoreHittablesOf(rayIn);
        }
    }
}