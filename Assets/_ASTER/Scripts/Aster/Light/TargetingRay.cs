using Aster.Entity.Enemy;
using UnityEngine;

namespace Aster.Light
{
    public class TargetingRay : LightRay, ILightRay
    {
        public static BaseLightHittable IgnoreHittable { get; set; } = null;

        private bool visible = false;

        public TargetingRay(bool activate = true) : base(activate)
        {
            ForceUpdate();
        }

        public TargetingRay(ILightRay source, bool activate = true, BaseLightHittable firstHittable = null) :
            base(source, activate)
        {
        }

        bool ILightRay.CheckIgnoreHittable(BaseLightHittable hittable)
        {
            if (hittable == null) return true;

            bool isEnemy          = hittable is EnemyHittable;
            bool isIgnoreHittable = (hittable == IgnoreHittable);

            if (!isIgnoreHittable && !hittable.IsTargetingOnly)
            {
                isIgnoreHittable |= IgnoreHittable != null && hittable.transform.IsChildOf(IgnoreHittable.transform);
            }

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