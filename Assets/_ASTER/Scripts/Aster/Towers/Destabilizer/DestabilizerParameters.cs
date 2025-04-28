using UnityEngine;

namespace Aster.Towers.Destabilizer
{
    [System.Serializable]
    public class DestabilizerParameters : BaseTowerParameters<DestabilizerParameters>
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

        public override DestabilizerParameters Clone()
        {
            return new DestabilizerParameters()
                   {
                       _coneAngle        = _coneAngle,
                       _intensityScale   = _intensityScale,
                       _emissionDistance = _emissionDistance,
                       _widthScale       = _widthScale,
                       _sineSpeed        = _sineSpeed
                   };
        }
    }
}