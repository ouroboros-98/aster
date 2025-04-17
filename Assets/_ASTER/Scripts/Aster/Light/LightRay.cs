using UnityEngine;

namespace Aster.Light
{
    public class LightRay : MonoBehaviour
    {
        
        private Vector3 _direction;

        public Vector3 GetDirection()
        {
            return this._direction;
        }
    }
}