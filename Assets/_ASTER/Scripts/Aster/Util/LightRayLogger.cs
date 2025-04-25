using Aster.Light;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Utils
{
    [System.Serializable]
    public class LightRayLogger
    {
        public ILightRay LightRay;

#if UNITY_EDITOR
        [ReadOnly, AllowNesting]                    public string  Type;
        [ReadOnly, AllowNesting]                    public Vector3 Origin;
        [ReadOnly, AllowNesting]                    public Vector3 EndPoint;
        [ReadOnly, AllowNesting]                    public Vector3 Direction;
        [ReadOnly, AllowNesting]                    public float   Intensity;
        [ReadOnly, AllowNesting]                    public float   Width;
        [ReadOnly, AllowNesting]                    public Color   Color;
#endif

        public void Update()
        {
            if (LightRay == null) return;

#if UNITY_EDITOR
            Type      = LightRay.GetType().Name;
            Origin    = LightRay.Origin;
            EndPoint  = LightRay.EndPoint;
            Direction = LightRay.Direction;
            Intensity = LightRay.Intensity;
            Width     = LightRay.Width;
            Color     = LightRay.Color;
#endif
        }
    }
}