using System;
using Aster.Light;
using Aster.Utils;
using Aster.Utils.Pool;

namespace Aster.Towers
{
    public class MirrorManipulator : LightManipulator<MirrorManipulator.ReflectedRay>
    {
        public class ReflectedRay : ContinuedRay<MirrorManipulation>
        {
            public ReflectedRay(LightHit source, MirrorManipulation manipulation) : base(source, manipulation)
            {
            }
        }

        private readonly Mirror               _mirror;
        private          IPool<LightRayObject>      _rayPool;
        private          MirrorManipulation _mirrorManipulation;

        public MirrorManipulator(Mirror mirror) : base(mirror)
        {
            _mirror = mirror;

            _rayPool              = RayPool.Instance;
            _mirrorManipulation = new MirrorManipulation(mirror.transform);
        }

        protected override ReflectedRay CreateManipulation(LightHit lightHit)
        {
            ReflectedRay reflectionData = new(lightHit, _mirrorManipulation);

            reflectionData.IgnoreHittable(_mirror);

            return reflectionData;
        }

        protected override ReflectedRay UpdateManipulation(LightHit hit, ReflectedRay reflection)
        {
            reflection.UpdateTransformation(hit);

            return reflection;
        }

        protected override void DestroyManipulation(LightRay ray, ReflectedRay reflection)
        {
            reflection.Destroy();
        }
    }
}