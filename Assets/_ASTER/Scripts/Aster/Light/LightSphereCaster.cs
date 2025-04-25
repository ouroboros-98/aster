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

            RaycastHit[] hits = Physics.SphereCastAll(ray.Origin, ray.Width / 2f, ray.Direction);

            Debug.DrawRay(ray.Origin, ray.Direction * LightRay.MAX_DISTANCE, Color.red, 0.005f);

            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider.ScanForComponent(out BaseLightHittable hittable, parents: true,
                                                   children: true))
                    continue;
                if (hit.distance == 0 && hit.point == Vector3.zero) continue;

                result.Add(new(ray, hit.point, hittable));
            }

            result.Sort((x, y) => x.Distance.CompareTo(y.Distance));

            return result;
        }
    }
}