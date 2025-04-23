using System;
using Aster.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Aster.Light
{
    public abstract partial class RayManipulation
    {
        public abstract void Apply(LightHit hit, LightRay rayIn, LightRay rayOut);
        public          void Apply(LightHit hit, LightRay rayOut) => Apply(hit, hit.Ray, rayOut);
        public          void Apply(LightHit hit) => Apply(hit, hit.Ray);

        public LightRay GetManipulatedRay(LightHit hit, LightRay rayIn)
        {
            LightRay rayOut = new LightRay(rayIn, activate: false);

            Apply(hit, rayIn, rayOut);

            return rayOut;
        }

        public LightRay ContinueRay(LightHit hit)
        {
            LightRay ray             = hit.Ray;
            LightRay continuationRay = hit.Ray.ContinueRay();

            Apply(hit, continuationRay);

            continuationRay.ExistsWhen(ray.CheckExists);

            return continuationRay;
        }

        public virtual CompositeRayManipulation Append(RayManipulation manipulation) => new(manipulation);
    }
}