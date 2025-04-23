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

        protected override float ApplyIntensity(LightHit hit, LightRay ray, float intensity) =>
            intensity / Parameters.SplitCount;

        protected override Vector3 ApplyDirection(LightHit hit, LightRay ray, Vector3 direction)
        {
            // float offset  = _angleOffset;
            // float baseDir = ((Vector3)(ray.Direction)).XZ().ToAngle();

            return Quaternion.AngleAxis(_angleOffset, Vector3.up) * ray.Direction;
        }

        protected override Vector3 ApplyOrigin(LightHit hit, LightRay ray, Vector3 origin)
        {
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