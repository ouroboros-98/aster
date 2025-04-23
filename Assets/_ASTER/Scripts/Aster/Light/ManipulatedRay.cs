using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Aster.Light
{
    public abstract class ManipulatedRay : LightRay
    {
        protected LightRay SourceRay;

        public ManipulatedRay(LightRay source) : base(source)
        {
            SourceRay = source;
        }

        public abstract void UpdateTransformation(LightHit hit);
    }

    public class ManipulatedRay<TTransformation> : ManipulatedRay where TTransformation : RayManipulation
    {
        private TTransformation _manipulation;

        public ManipulatedRay(LightHit source, TTransformation manipulation) : base(source.Ray)
        {
            _manipulation = manipulation;

            UpdateTransformation(source);
        }

        public override void UpdateTransformation(LightHit hit)
        {
            if (!this.IsActive) return;

            _manipulation.Apply(hit, this);
        }
    }

    public class ContinuedRay<TTransformation> : ManipulatedRay<TTransformation>
        where TTransformation : RayManipulation
    {
        public ContinuedRay(LightHit source, TTransformation manipulation) : base(source, manipulation)
        {
            SourceRay.OnDestroy += Destroy;
        }
    }
}