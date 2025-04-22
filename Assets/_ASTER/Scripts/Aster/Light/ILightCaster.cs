using System.Collections.Generic;

namespace Aster.Light
{
    public interface ILightCaster
    {
        List<LightHit> GetHits(RayData ray);
    }
}