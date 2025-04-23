using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Aster.Light
{
    public abstract class TransformedRay : LightRay
    {
        protected LightRay SourceRay;

        public TransformedRay(LightRay source) : base(source)
        {
            SourceRay = source;
        }

        public abstract void UpdateTransformation(LightHit hit);
    }

    public class TransformedRay<TTransformation> : TransformedRay where TTransformation : RayTransformation
    {
        private TTransformation _transformation;

        public TransformedRay(LightHit source, TTransformation transformation) : base(source.Ray)
        {
            _transformation = transformation;

            UpdateTransformation(source);
        }

        public override void UpdateTransformation(LightHit hit)
        {
            if (!this.IsActive) return;

            _transformation.Apply(hit, this);
        }
    }

    public class ContinuedRay<TTransformation> : TransformedRay<TTransformation>
        where TTransformation : RayTransformation
    {
        public ContinuedRay(LightHit source, TTransformation transformation) : base(source, transformation)
        {
            SourceRay.OnDestroy += Destroy;
        }
    }
}