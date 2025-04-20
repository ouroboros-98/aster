using Aster.Light;
using UnityEngine;
using UnityEngine.Android;

namespace Aster.Towers
{
    public class Emitter : BaseTower
    {
        [SerializeField] private Vector3 mainLightPos; 
        [SerializeField] private float radiusFromMainLight;
        public override void OnLightRayHit(LightRay ray)
        {
            return;
        }
        
    }
}