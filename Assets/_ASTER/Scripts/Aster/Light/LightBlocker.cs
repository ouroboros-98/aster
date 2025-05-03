namespace Aster.Light
{
    public class LightBlocker : BaseLightHittable
    {
        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            return new LightHitContext(lightHit, blockLight: true);
        }
    }
}