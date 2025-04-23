using Aster.Light;
using UnityEngine;

namespace Aster.Towers
{
    public class MirrorTransformation : RayTransformation
    {
        private Transform _transform;

        public MirrorTransformation(Transform transform)
        {
            _transform = transform;
        }

        protected override float ApplyIntensity(LightHit hit, LightRay ray, float intensity) => intensity * .9f;

        protected override Vector3 ApplyDirection(LightHit hit, LightRay ray, Vector3 direction) =>
            GetReflectionDir(ray);

        protected override Vector3 ApplyOrigin(LightHit hit, LightRay ray, Vector3 origin) => hit.HitPoint;

        private Vector3 GetReflectionDir(LightRay ray) => Vector3.Reflect(ray.Direction, _transform.forward);
    }
}