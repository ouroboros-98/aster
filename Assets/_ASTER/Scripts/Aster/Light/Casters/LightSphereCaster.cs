using System.Collections.Generic;
using Aster.Utils;
using UnityEngine;

namespace Aster.Light
{
    public class LightSphereCaster : ILightCaster
    {
        public List<LightHit> GetHits(ILightRay ray)
        {
            List<LightHit> result = new();

            RaycastHit[] hits = Physics.SphereCastAll(ray.Origin, ray.Width / 2f, ray.Direction, ray.MaxDistance);

            Debug.DrawRay(ray.Origin, ray.Direction * ray.MaxDistance, Color.red, 0.005f);

            foreach (RaycastHit hit in hits)
            {
                if (LightRayIgnore.IgnoreList.Contains(hit.transform))
                    continue;

                if (!hit.collider.ScanForComponent(out BaseLightHittable hittable, parents: true,
                                                   children: true))
                    continue;
                if (hit.distance == 0 && hit.point == Vector3.zero) continue;

                // Project the hit point to the ray
                Vector3 actualHitPoint = ray.Origin + ray.Direction * (hit.distance + ray.Width / 2f);

                result.Add(new(ray, actualHitPoint, hittable));
            }

            result.Sort((x, y) => x.Distance.CompareTo(y.Distance));

            return result;
        }
    }
}