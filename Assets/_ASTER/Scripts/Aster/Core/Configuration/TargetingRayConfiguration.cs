using Aster.Utils.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        [SerializeField, BoxedProperty] private TargetingConfiguration targeting;
        public                                  TargetingConfiguration Targeting => targeting;

        [System.Serializable]
        public class TargetingConfiguration
        {
            [SerializeField] private bool  enableRay = false;
            [SerializeField] private Color rayColor  = new(0.8039216f, 0.1568628f, 0.1098039f);

            public bool  EnableRay => enableRay;
            public Color RayColor  => rayColor;
        }
    }
}