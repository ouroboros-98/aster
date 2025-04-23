using Aster.Entity.Enemy;
using UnityEngine;

namespace Aster.Light
{
    public class TargetingRay : LightRay
    {
        private static readonly Color _targetingRayColor = new(0.8039216f, 0.1568628f, 0.1098039f);
        public override         Color Color => _targetingRayColor;

        public TargetingRay() : base()
        {
            base.Color = _targetingRayColor;
            ForceUpdate();
        }

        public TargetingRay(LightRay source) : base(source)
        {
            base.Color = _targetingRayColor;
            ForceUpdate();
        }

        public override bool CheckIgnoreHittable(BaseLightHittable hittable)
        {
            bool isEnemy = hittable is EnemyHittable;
            return isEnemy || base.CheckIgnoreHittable(hittable);
        }
    }
}