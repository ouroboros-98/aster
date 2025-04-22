using Aster.Light;

namespace Aster.Towers
{
    public class SplittedRay : ContinuedRay<SplitterRayTransformation>
    {
        public SplittedRay(LightHit source, SplitterRayTransformation transformation) : base(source, transformation)
        {
        }
    }
}