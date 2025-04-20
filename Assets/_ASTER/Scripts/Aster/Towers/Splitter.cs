using Aster.Light;
using Aster.Utils.Pool;
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
        public override void OnLightRayHit(LightRay ray)
        {
            Vector3 hitPosition = ray.GetHitPosition();
            Vector3 baseDir = ray.GetDirection();
            
            // Calculate the angle offsets for the 4 beams.
            // Offsets are chosen so that they evenly span the user set cone.
            float halfCone = splitConeAngle * 0.5f;
            float[] angleOffsets = new float[4]
            {
                -halfCone, -halfCone * 0.33f, halfCone * 0.33f, halfCone
            };

            // Create 4 new light rays with different colors.
            for (int i = 0; i < 4; i++)
            {
                Vector3 newDir = Quaternion.AngleAxis(angleOffsets[i], Vector3.up) * baseDir;
                CreateNewLight(newDir, hitPosition, splitColors[i], ray);
            }
        }

        private void CreateNewLight(Vector3 newLightDirection, Vector3 hitPosition, Color color, LightRay originatingRay)
        {
            // Offset the spawn position so the new ray starts just beyond the splitter.
            Vector3 spawnPosition = hitPosition + newLightDirection.normalized * spawnOffsetDistance;
            // Retrieve a new LightRay from the ray pool
            LightRay lightRay = RayPool.Instance.Get();

            // Initialize the LightRay with the new direction and color.
            lightRay.Initialize(spawnPosition, newLightDirection, color, 1, originatingRay);
        }
    }
}