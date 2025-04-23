using Aster.Light;
using UnityEngine;

namespace Aster.Towers
{
    public class MirrorManipulation : CompositeRayManipulation
    {
        private static readonly RayManipulation ApplyIntensity = Manipulate.Intensity(
             rayIn => rayIn.Intensity * .9f
            );

        private static readonly RayManipulation ApplyOrigin = Manipulate.Origin(
                                                                                hit => hit.HitPoint
                                                                               );

        private readonly RayManipulation ApplyDirection;

        private Transform _transform;

        public MirrorManipulation(Transform transform) : base()
        {
            _transform = transform;

            ApplyDirection = Manipulate.Direction(ReflectDirection);

            Append(ApplyIntensity);
            Append(ApplyOrigin);
            Append(ApplyDirection);
        }

        private Vector3 ReflectDirection(ILightRay rayIn)
        {
            return Vector3.Reflect(rayIn.Direction, _transform.forward);
        }
    }
}