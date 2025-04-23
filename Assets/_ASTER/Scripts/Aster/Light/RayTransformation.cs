using System;
using Aster.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Aster.Light
{
    public abstract class RayTransformation
    {
        public void Apply(LightHit hit) => Apply(hit, hit.Ray);

        public virtual void Apply(LightHit hit, LightRay lightRayOut)
        {
            LightRay lightRayIn = hit.Ray;

            lightRayOut.Direction = ApplyDirection(hit, lightRayIn, lightRayIn.Direction);
            lightRayOut.Origin    = ApplyOrigin(hit, lightRayIn, lightRayIn.Origin);
            lightRayOut.Intensity = ApplyIntensity(hit, lightRayIn, lightRayIn.Intensity);
            lightRayOut.Width     = ApplyWidth(hit, lightRayIn, lightRayIn.Width);
            lightRayOut.Color     = ApplyColor(hit, lightRayIn, lightRayIn.Color);
        }

        public LightRay ContinueRay(LightHit hit)
        {
            LightRay ray             = hit.Ray;
            LightRay continuationRay = hit.Ray.ContinueRay();

            Apply(hit, continuationRay);

            continuationRay.ExistsWhen(ray.CheckExists);

            return continuationRay;
        }

        protected virtual Vector3 ApplyOrigin(LightHit    hit, LightRay ray, Vector3 origin)    => origin;
        protected virtual Vector3 ApplyDirection(LightHit hit, LightRay ray, Vector3 direction) => direction;
        protected virtual float   ApplyIntensity(LightHit hit, LightRay ray, float   intensity) => intensity;
        protected virtual float   ApplyWidth(LightHit     hit, LightRay ray, float   width)     => width;
        protected virtual Color   ApplyColor(LightHit     hit, LightRay ray, Color   color)     => color;
    }
}