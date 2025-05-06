using System;
using Aster.Utils.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        private LightrayConfig lightrays = new();
        public  LightrayConfig Lightrays => lightrays;

        [BoxGroup("Light Rays")]
        [Range(0, 100)]
        [SerializeField]
        private float maxDistance = 100f;

        public class LightrayConfig
        {
            public AsterConfiguration _config;
            public float              MaxDistance => _config.maxDistance;
        }
    }
}