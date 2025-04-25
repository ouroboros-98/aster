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

        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            enemyController.LightHit(lightHit);

            return new(lightHit);
        }
    }
}