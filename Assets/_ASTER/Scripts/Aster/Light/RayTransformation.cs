using System;
using Aster.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Aster.Light
{
    public abstract class RayTransformation
    {
        public void Apply(LightHit hit) => Apply(hit, hit.Ray);

        public virtual void Apply(LightHit hit, RayData rayOut)
        {
            RayData rayIn = hit.Ray;

            rayOut.Direction = ApplyDirection(hit, rayIn, rayIn.Direction);
            rayOut.Origin    = ApplyOrigin(hit, rayIn, rayIn.Origin);
            rayOut.Intensity = ApplyIntensity(hit, rayIn, rayIn.Intensity);
            rayOut.Width     = ApplyWidth(hit, rayIn, rayIn.Width);
            rayOut.Color     = ApplyColor(hit, rayIn, rayIn.Color);
        }

        public RayData ContinueRay(LightHit hit)
        {
            RayData ray             = hit.Ray;
            RayData continuationRay = hit.Ray.ContinueRay();

            Apply(hit, continuationRay);

            continuationRay.ExistsWhen(ray.CheckExists);

            return continuationRay;
        }

        protected virtual Vector3 ApplyOrigin(LightHit    hit, RayData ray, Vector3 origin)    => origin;
        protected virtual Vector3 ApplyDirection(LightHit hit, RayData ray, Vector3 direction) => direction;
        protected virtual float   ApplyIntensity(LightHit hit, RayData ray, float   intensity) => intensity;
        protected virtual float   ApplyWidth(LightHit     hit, RayData ray, float   width)     => width;
        protected virtual Color   ApplyColor(LightHit     hit, RayData ray, Color   color)     => color;
    }
}