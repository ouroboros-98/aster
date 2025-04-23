using System.Linq;
using Aster.Light;
using UnityEngine;

namespace Aster.Utils
{
    public static class RayDataExtensions
    {
        public static LightRay ContinueRay(this LightRay ray,          Vector3? origin = null, Vector3? direction = null,
                                          Color?       color = null, float?   width  = null, float?   intensity = null)
        {
            LightRay newRay = new()
                             {
                                 Origin    = origin    ?? ray.EndPoint,
                                 Direction = direction ?? ray.Direction,
                                 Color     = color     ?? ray.Color,
                                 Width     = width     ?? ray.Width,
                                 Intensity = intensity ?? ray.Intensity
                             };

            ray.ExistencePredicates.ToList().ForEach(newRay.ExistsWhen);

            return newRay;
        }
    }
}