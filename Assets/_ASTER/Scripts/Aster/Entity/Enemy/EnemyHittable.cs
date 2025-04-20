using Aster.Light;
using UnityEngine;

namespace Aster.Entity.Enemy
{
    public class EnemyHittable : BaseLightHittable
    {
        [SerializeField] private EnemyController enemyController;

        private void Awake()
        {
            Reset();
        }

        private void Reset()
        {
            ValidateComponent(ref enemyController);
        }

        public override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            enemyController.LightHit();

            return new(lightHit);
        }
    }
}