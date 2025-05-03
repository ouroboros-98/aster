using System.Linq;
using Aster.Light;
using UnityEngine;

namespace Aster.Utils
{
    public static class RayExtensions
    {
        public static float Distance(this ILightRay ray)
        {
            return Vector3.Distance(ray.Origin, ray.EndPoint);
        }
    }
}