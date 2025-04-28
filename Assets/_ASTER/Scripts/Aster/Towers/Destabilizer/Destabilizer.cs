using Aster.Light;
using Aster.Utils;
using Aster.Utils.Attributes;
using UnityEngine;

namespace Aster.Towers.Destabilizer
{
    public class Destabilizer : BaseTower
    {
        [SerializeField, BoxedProperty] private DestabilizerParameters _parameters = new();

        private         LightReceiver  _lightReceiver;
        public override LightReceiver  LightReceiver => _lightReceiver;
        private         RayManipulator _manipulator;

        private DestabilizerManipulation _manipulation;

        protected override void Awake()
        {
            _lightReceiver = new LightReceiver();
            _manipulation  = new DestabilizerManipulation(_parameters, transform);
            _manipulator   = new RayManipulator(_lightReceiver, transform, _manipulation);
        }
    }

    [System.Serializable]
    public class DestabilizerParameters
    {
        [SerializeField]              private float _coneAngle        = 10f;
        [SerializeField, Range(0, 1)] private float _intensityScale   = .95f;
        [SerializeField]              private float _emissionDistance = .3f;
        [SerializeField, Range(0, 1)] private float _widthScale       = .5f;
        [SerializeField]              private float _sineSpeed        = 1f;

        public float ConeAngle        => _coneAngle;
        public float IntensityScale   => _intensityScale;
        public float EmissionDistance => _emissionDistance;
        public float WidthScale       => _widthScale;
        public float SineSpeed        => _sineSpeed;
    }

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