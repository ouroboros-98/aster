using Aster.Light;
using Aster.Utils.Pool;
using UnityEngine;
using UnityEngine.Android;

namespace Aster.Towers
{
    public class Emitter : BaseTower
    {
        [SerializeField] private Vector3 mainLightPos; 
        public override void OnLightRayHit(LightRay ray)
        {
            return;
        }
    }
}