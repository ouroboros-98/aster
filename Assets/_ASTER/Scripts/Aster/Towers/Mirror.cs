using Aster.Light;
using UnityEngine;

namespace Aster.Towers
{
    public class Mirror : BaseTower
    {
        public override void OnLightRayHit(LightRay ray)
        {
            Vector3 newLightDirection = GetNewLightDirection(ray);
            CreateNewLight(newLightDirection);
            
            
        }

        private Vector3 GetNewLightDirection(LightRay ray)
        {
            float angleDeg = this.angle;
            float angleRad = angleDeg * Mathf.Deg2Rad;
            Vector3 normalVec = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad));
            var newLightDirection = Vector3.Reflect(ray.GetDirection(), normalVec);
            return newLightDirection;
        }

        private void CreateNewLight(Vector3 newLightDirection)
        {
            //todo: make this function
        }
    }
}