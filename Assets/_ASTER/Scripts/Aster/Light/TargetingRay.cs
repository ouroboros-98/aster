using Aster.Entity.Enemy;
using UnityEngine;

namespace Aster.Light
{
    public class TargetingRay : LightRay, ILightRay
    {
        private static readonly Color _targetingRayColor = new(0.8039216f, 0.1568628f, 0.1098039f);
        public override         Color Color => _targetingRayColor;

        public TargetingRay(bool activate = true) : base(activate)
        {
            base.Color = _targetingRayColor;
            ForceUpdate();
        }

        public TargetingRay(ILightRay source, bool activate = true) : base(source, activate)
        {
            base.Color = _targetingRayColor;
        }

        bool ILightRay.CheckIgnoreHittable(BaseLightHittable hittable)
        {
            bool isEnemy = hittable is EnemyHittable;
            return isEnemy || base.CheckIgnoreHittable(hittable);
        }

        ILightRay ILightRay.Clone(bool activate = true) => new TargetingRay(this, activate);

        ILightRay ILightRay.Continue()
        {
            if (!IsActive) return null;

            TargetingRay continuationRay = new TargetingRay(this);
            OnDestroy += continuationRay.Destroy;

            return continuationRay;
        }
    }
}