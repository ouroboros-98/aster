using Aster.Light;
using Aster.Utils;
using UnityEngine;

namespace Aster.Towers.Destabilizer
{
    public class DestabilizerManipulation : RayManipulation
    {
        private readonly Transform              _transform;
        private readonly DestabilizerParameters _parameters;

        public DestabilizerManipulation(DestabilizerParameters parameters, Transform transform)
        {
            _parameters = parameters;
            _transform  = transform;
        }

        public override void Apply(LightHit hit, ILightRay rayIn, ILightRay rayOut)
        {
            ApplyOrigin(rayIn, rayOut);
            ApplyIntensity(rayIn, rayOut);
            ApplyDirection(rayIn, rayOut);
            ApplyWidth(rayIn, rayOut);
        }

        private void ApplyWidth(ILightRay rayIn, ILightRay rayOut)
        {
            rayOut.Width = rayIn.Width * _parameters.WidthScale;
        }

        private void ApplyOrigin(ILightRay rayIn, ILightRay rayOut)
        {
            rayOut.Origin = _transform.position.With(y: rayOut.Origin.y) +
                            _transform.forward * _parameters.EmissionDistance;
        }

        private void ApplyDirection(ILightRay rayIn, ILightRay rayOut)
        {
            Angle   offset    = GenerateOffset();
            Vector3 direction = _transform.forward;
            direction        = Quaternion.Euler(0, offset, 0) * direction;
            rayOut.Direction = direction;
        }

        private void ApplyIntensity(ILightRay rayIn, ILightRay rayOut)
        {
            rayOut.Intensity = rayIn.Intensity * _parameters.IntensityScale;
        }

        private float GenerateOffset()
        {
            Angle halfCone = _parameters.ConeAngle / 2f;

            // float multiplier = Random.Range(-1f, 1f);

            float multiplier = Mathf.Sin(Time.fixedTime * _parameters.SineSpeed);

            return multiplier * halfCone;
        }
    }
}