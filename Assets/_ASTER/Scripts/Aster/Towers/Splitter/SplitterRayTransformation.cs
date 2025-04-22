using Aster.Light;
using Aster.Utils;
using UnityEngine;

namespace Aster.Towers
{
    public class SplitterRayTransformation : RayTransformation
    {
        private readonly int      _index;
        private readonly Splitter _splitterTower;

        private float _angleOffset;

        private SplitterParameters Parameters => _splitterTower.Parameters;

        public SplitterRayTransformation(Splitter splitterTower, int index) :
            base()
        {
            _splitterTower = splitterTower;
            _index         = index;
            _angleOffset   = CalculateAngleOffset();
        }

        protected override float ApplyIntensity(LightHit hit, RayData ray, float intensity) =>
            intensity / Parameters.SplitCount;

        protected override Vector3 ApplyDirection(LightHit hit, RayData ray, Vector3 direction)
        {
            // float offset  = _angleOffset;
            // float baseDir = ((Vector3)(ray.Direction)).XZ().ToAngle();

            return Quaternion.AngleAxis(_angleOffset, Vector3.up) * ray.Direction;
        }

        protected override Vector3 ApplyOrigin(LightHit hit, RayData ray, Vector3 origin) =>
            hit.HitPoint + ray.Direction * Parameters.SpawnOffsetDistance;


        private Angle CalculateAngleOffset()
        {
            Angle halfCone    = Parameters.SplitConeAngle * 0.5f;
            Angle firstOffset = -halfCone;
            Angle delta       = Parameters.SplitConeAngle / (Parameters.SplitCount);

            Angle offset = firstOffset + _index * delta;
            return offset;
        }
    }
}