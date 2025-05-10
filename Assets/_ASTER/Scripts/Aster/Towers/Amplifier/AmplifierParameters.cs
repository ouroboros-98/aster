using System;
using UnityEngine;

namespace Aster.Towers.Amplifier
{
    [Serializable]
    public class AmplifierParameters : BaseTowerParameters<AmplifierParameters>
    {
        [SerializeField] private float newMaxDistance = 10f;
        [SerializeField] private float intensityScale = 1f;

        public float NewMaxDistance => newMaxDistance;
        public float IntensityScale => intensityScale;

        public override AmplifierParameters Clone()
        {
            return new()
                   {
                       newMaxDistance = newMaxDistance,
                       intensityScale = intensityScale
                   };
        }
    }
}