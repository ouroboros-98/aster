using System;
using Aster.Core.Interactions.Grab;
using Aster.Utils;

namespace Aster.Light
{
    public class BlockLight : BaseLightHittable, IDisableOnGrab
    {
        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            return new LightHitContext(lightHit, blockLight: true);
        }
    }
}