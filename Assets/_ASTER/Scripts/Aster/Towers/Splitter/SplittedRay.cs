using Aster.Light;

namespace Aster.Towers
{
    public class SplittedRay : ContinuedRay<SplitterManipulation>
    {
        public SplittedRay(LightHit source, SplitterManipulation manipulation) : base(source, manipulation)
        {
        }
    }
}