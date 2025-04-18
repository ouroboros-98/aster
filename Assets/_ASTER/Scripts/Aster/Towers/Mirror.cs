using Aster.Light;
using UnityEngine;

namespace Aster.Towers
{
    public class Mirror : BaseTower
    {
        [SerializeField] private LightRay lightRayPrefab;
        public override void OnLightRayHit(LightRay ray)
        {
            Vector3 newLightDirection = GetNewLightDirection(ray);
            CreateNewLight(newLightDirection,ray);
        }

        private Vector3 GetNewLightDirection(LightRay ray)
        {
            float angleDeg = this.angle;
            float angleRad = angleDeg * Mathf.Deg2Rad;
            Vector3 normalVec = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad));
            var newLightDirection = Vector3.Reflect(ray.GetDirection(), normalVec);
            return newLightDirection;
        }

        private void CreateNewLight(Vector3 newLightDirection, LightRay ray)
        {
            Vector3 hitPosition = ray.GetHitPosition();
            // Instantiate the LightRay prefab
            var lightRay = Instantiate(lightRayPrefab,hitPosition, Quaternion.identity);
        
            // Initialize the LightRay
            lightRay.Initialize(hitPosition, newLightDirection, Color.yellow, 1,ray);
        }
    }
}