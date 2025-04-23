using Aster.Light;
using Aster.Utils;
using UnityEngine;

namespace Aster.Towers
{
    public class SplitterManipulation : CompositeRayManipulation
    {
        private readonly int      _index;
        private readonly Splitter _splitterTower;

        private float _angleOffset;

        private SplitterParameters Parameters => _splitterTower.Parameters;

        public SplitterManipulation(Splitter splitterTower, int index) :
            base()
        {
            _splitterTower = splitterTower;
            _index         = index;
            _angleOffset   = CalculateAngleOffset();

            Append(Manipulate.Intensity(ApplyIntensity));
            Append(Manipulate.Direction(ApplyDirection));
            Append(Manipulate.Origin(ApplyOrigin));
        }

        private float ApplyIntensity(ILightRay rayIn) =>
            rayIn.Intensity / Parameters.SplitCount;

        private Vector3 ApplyDirection(ILightRay ray) =>
            Quaternion.AngleAxis(_angleOffset, Vector3.up) * ray.Direction;

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