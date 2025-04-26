using Aster.Light;
using Aster.Towers;
using Aster.Utils;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Aster.Entity.Enemy
{
    public class EnemyHittable : BaseLightHittable
    {
        [SerializeField] private EnemyController enemyController;

        private LightReceiver        lightReceiver;
        private EnemyRayManipulation enemyRayManipulation = new();
        private RayManipulator       enemyRayManipulator;

        private void Awake()
        {
            Reset();
        }

        private void Reset()
        {
            ValidateComponent(ref enemyController);

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

        public override void OnLightRayExit(LightRayObject rayObject)
        {
            lightReceiver.Deregister(rayObject.Data);
        }
    }

    [System.Serializable]
    public class EnemyRayManipulation : RayManipulation
    {
        [SerializeField] private float intensityScale = .8f;

        public override void Apply(LightHit hit, ILightRay rayIn, ILightRay rayOut)
        {
            rayOut.Origin    = hit.HitPoint;
            rayOut.Direction = rayIn.Direction;
            rayOut.Intensity = rayIn.Intensity * intensityScale;
            rayOut.EndPoint  = hit.HitPoint + rayIn.Direction * LightRay.MAX_DISTANCE;

            rayOut.IgnoreHittablesOf(rayIn);
        }
    }
}