using Aster.Core;
using Aster.Core.Entity;
using Aster.Light;

namespace Aster.Towers
{
    public abstract class BaseTower : BaseLightHittable
    {
        protected EntityHP HP;
        protected float    angle;

        protected LightReceiver lightReceiver = new();
        public LightReceiver LightReceiver => lightReceiver;

        public override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            lightReceiver.Register(lightHit);

            return CreateHitContext(lightHit);
        }

        protected virtual LightHitContext CreateHitContext(LightHit hit)
        {
            return new(hit, blockLight: true);
        }

        public override void OnLightRayExit(LightRay ray)
        {
            lightReceiver.Deregister(ray.Data);
        }
    }
}