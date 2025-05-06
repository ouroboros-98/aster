using Aster.Utils.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        private TargetingConfiguration targeting = new();

        public TargetingConfiguration Targeting => targeting;

        [BoxGroup("Targeting")]
        [SerializeField]
        private bool rotateWithoutTargeting = false;

        [BoxGroup("Targeting")]
        [SerializeField]
        private bool enableRay = false;

        [BoxGroup("Targeting")]
        [SerializeField]
        private Color rayColor = new(0.8039216f, 0.1568628f, 0.1098039f);

        public class TargetingConfiguration
        {
            public AsterConfiguration _config;

            public bool  RotateWithoutTargeting => _config.rotateWithoutTargeting;
            public bool  EnableRay              => _config.enableRay && !_config.rotateWithoutTargeting;
            public Color RayColor               => _config.rayColor;
        }
    }
}