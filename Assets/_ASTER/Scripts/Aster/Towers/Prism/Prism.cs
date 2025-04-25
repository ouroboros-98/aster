using Aster.Light;
using UnityEngine;

namespace Aster.Towers.Prism
{
    public class Prism : BaseTower, IDuplicatable
    {
        private LightReceiver lightReceiver;

        public override LightReceiver LightReceiver => lightReceiver;

        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            if (lightHit.Ray.Color != Color.white || lightHit.Ray is not TargetingRay)
                return new LightHitContext(lightHit, blockLight: false);

            return base.OnLightRayHit(lightHit);
        }

        protected override void Reset()
        {
            base.Reset();

            lightReceiver = new();
        }
    }
}