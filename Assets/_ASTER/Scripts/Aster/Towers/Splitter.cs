using System.Collections.Generic;
using Aster.Light;
using Aster.Utils.Pool;
using JetBrains.Annotations;
using UnityEngine;

namespace Aster.Towers
{
    public class Splitter : BaseTower
    {
        [SerializeField] private float splitConeAngle = 45f;

        [SerializeField] private Color[] splitColors = new Color[4]
                                                       {
                                                           Color.red, Color.green, Color.blue, Color.white
                                                       };

        [SerializeField] private float spawnOffsetDistance = 1f; // Offset to spawn new rays

        private Dictionary<LightRay, List<SplitLightData>> splitRays = new();

        public override LightHitContext OnLightRayHit(LightHit hit)
        {
            LightRay ray = hit.Ray;

            if (!splitRays.ContainsKey(ray))
            {
                splitRays[ray] = CreateDefaultList(hit);
            }

            splitRays[ray].ForEach(splitLightData => splitLightData.Update(hit));

            return new LightHitContext(hit, blockLight: true);
        }

        private List<SplitLightData> CreateDefaultList(LightHit hit)
        {
            List<SplitLightData> result = new();

            LightRay ray = hit.Ray;

            Vector3 hitPosition = hit.HitPoint;
            Vector3 baseDir     = hit.RayDirection;

            float[] angleOffsets = CalculateAngleOffsets();

            for (int i = 0; i < 4; i++)
            {
                SplitLightData splitLightData = new SplitLightData(hit, splitColors[i], angleOffsets[i],
                                                                   spawnOffsetDistance);
                result.Add(splitLightData);
            }

            return result;
        }

        private float[] CalculateAngleOffsets()
        {
            float halfCone = splitConeAngle * 0.5f;
            float[] angleOffsets = new float[4]
                                   {
                                       -halfCone, -halfCone * 0.33f, halfCone * 0.33f, halfCone
                                   };
            return angleOffsets;
        }

        private void CreateNewLight(Vector3  newLightDirection, Vector3 hitPosition, Color color,
                                    LightRay originatingRay)
        {
            Vector3  spawnPosition = hitPosition + newLightDirection.normalized * spawnOffsetDistance;
            LightRay lightRay      = RayPool.Instance.Get();

            lightRay.Initialize(spawnPosition, newLightDirection, color, 1, originatingRay);
        }

        public override void OnLightRayExit(LightRay ray)
        {
            if (!splitRays.ContainsKey(ray)) return;

            splitRays[ray].ForEach(splitLightData => splitLightData.Destroy());
            splitRays.Remove(ray);
        }
    }

    public class SplitLightData
    {
        private readonly Color    _color;
        private readonly float    _angleOffset;
        private readonly float    _originOffset;
        private readonly LightRay _lightRay;

        public SplitLightData(LightHit hit, Color color, float angleOffset, float originOffset)
        {
            _color        = color;
            _angleOffset  = angleOffset;
            _originOffset = originOffset;

            _lightRay = CreateRay(hit);
        }

        private LightRay CreateRay(LightHit hit)
        {
            Vector3 newDir = CalculateDirection(hit);
            Vector3 origin = CalculateOrigin(hit, newDir);

            LightRay lightRay = RayPool.Instance.Get();
            lightRay.Initialize(origin, newDir, _color, 1, hit.Ray);

            return lightRay;
        }

        private Vector3 CalculateOrigin(LightHit hit, Vector3 newDir) =>
            hit.HitPoint + newDir.normalized * _originOffset;

        private Vector3 CalculateDirection(LightHit hit) =>
            Quaternion.AngleAxis(_angleOffset, Vector3.up) * hit.RayDirection;

        public void Update(LightHit hit)
        {
            _lightRay.Direction = CalculateDirection(hit);
            _lightRay.Origin    = CalculateOrigin(hit, _lightRay.Direction);
        }

        public void Destroy()
        {
            RayPool.Instance.Return(_lightRay);
        }
    }
}