using Aster.Entity.Enemy;
using UnityEngine;

namespace Aster.Light
{
    public class TargetingRay : LightRay, ILightRay
    {
        public static BaseLightHittable IgnoreHittable { get; set; } = null;

        private static readonly Color _targetingRayColor = new(0.8039216f, 0.1568628f, 0.1098039f);
        public override         Color Color => _targetingRayColor;

        private bool visible = false;

        public TargetingRay(bool activate = true) : base(activate)
        {
            base.Color = _targetingRayColor;
            ForceUpdate();
        }

        public TargetingRay(ILightRay source, bool activate = true, BaseLightHittable firstHittable = null) :
            base(source, activate)
        {
            base.Color = _targetingRayColor;
        }

        bool ILightRay.CheckIgnoreHittable(BaseLightHittable hittable)
        {
            bool isEnemy          = hittable is EnemyHittable;
            bool isIgnoreHittable = hittable != null && hittable == IgnoreHittable;
            return isEnemy || isIgnoreHittable || base.CheckIgnoreHittable(hittable);
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