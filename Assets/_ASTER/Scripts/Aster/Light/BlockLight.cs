using System;
using Aster.Utils;

namespace Aster.Light
{
    public class BlockLight : BaseLightHittable
    {
        

        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            return new LightHitContext(lightHit, blockLight: true);
        }
    }
}