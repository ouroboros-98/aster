using Aster.Core;
using Aster.Light;
using Aster.Utils;
using UnityEngine;

namespace Aster.Towers
{
    public class SplitterManipulation : RayManipulation
    {
        private SplitterParameters Parameters => _splitterTower.Parameters;

        private readonly int      _index;
        private readonly Splitter _splitterTower;

        private float _angleOffset;
        private int   _splitCount;
        private float _splitConeAngle;

        private readonly AsterConfiguration Config;

        public SplitterManipulation(Splitter splitterTower, int index) :
            base()
        {
            _splitterTower  = splitterTower;
            _index          = index;
            _splitConeAngle = Parameters.SplitConeAngle;
            _splitCount     = Parameters.SplitCount;
            _angleOffset    = CalculateAngleOffset();
            Config          = AsterConfiguration.Instance;
        }

        public override void Apply(LightHit hit, ILightRay rayIn, ILightRay rayOut)
        {
            rayOut.Intensity   = rayIn.Intensity / Parameters.SplitCount;
            rayOut.MaxDistance = Config.Lightrays.MaxDistance;
            rayOut.Direction   = ApplyDirection(rayIn);
            rayOut.Origin      = ApplyOrigin(rayIn);
            rayOut.Color       = ApplyColor(rayIn);
        }

        private Color ApplyColor(ILightRay ray)
        {
            if (!Parameters.Refract) return ray.Color;

            Color color = Color.HSVToRGB(_index / (float)Parameters.SplitCount, 1, 1);
            return color;
        }

        private Vector3 ApplyDirection(ILightRay ray)
        {
            if (Parameters.SplitCount != _splitCount ||
                !Mathf.Approximately(Parameters.SplitConeAngle, _splitConeAngle))
            {
                _angleOffset    = CalculateAngleOffset();
                _splitCount     = Parameters.SplitCount;
                _splitConeAngle = Parameters.SplitConeAngle;
            }

            return Quaternion.AngleAxis(_angleOffset + Parameters.DirectionOffset, Vector3.up) *
                   _splitterTower.transform.forward;
        }

        private Vector3 ApplyOrigin(ILightRay ray)
        {
            Vector3 origin              = ray.Origin;
            Vector3 towerOrigin         = _splitterTower.transform.position;
            Vector3 adjustedTowerOrigin = towerOrigin.With(y: origin.y);

            return adjustedTowerOrigin + ray.Direction * Parameters.SpawnOffsetDistance;
        }

        private float CalculateAngleOffset()
        {
            float halfCone    = Parameters.SplitConeAngle * 0.5f;
            float firstOffset = -halfCone;
            float delta       = Parameters.SplitConeAngle / (Parameters.SplitCount - 1);

            Angle offset = firstOffset + _index * delta;
            return offset;
        }
    }
}