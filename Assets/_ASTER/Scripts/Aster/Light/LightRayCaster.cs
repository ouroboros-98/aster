using System.Collections.Generic;
using Aster.Utils;
using UnityEngine;

namespace Aster.Light
{
    public class LightRayCaster : ILightCaster
    {
        public List<LightHit> GetHits(ILightRay ray)
        {
            List<LightHit> result = new();

            RaycastHit[] hits = Physics.RaycastAll(ray.Origin, ray.Direction, LightRay.MAX_DISTANCE);

            Debug.DrawRay(ray.Origin, ray.Direction * LightRay.MAX_DISTANCE, Color.red, 0.005f);

            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider.ScanForComponents(out BaseLightHittable[] hittables, parents: true, children: true))
                    continue;

                foreach (var hittable in hittables)
                {
                    result.Add(new(ray, hit.point, hittable));
                }
            }

            result.Sort((x, y) => x.Distance.CompareTo(y.Distance));

            return result;
        }
    }
}