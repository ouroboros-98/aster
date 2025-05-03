using System;
using Aster.Utils.Attributes;
using UnityEngine;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        [SerializeField, BoxedProperty] private LightrayConfig lightrays = new();
        public                                  LightrayConfig Lightrays => lightrays;

        [System.Serializable]
        public class LightrayConfig
        {
            [SerializeField, Range(0, 100)] private float maxDistance = 100f;

            public float MaxDistance => maxDistance;
        }
    }
}