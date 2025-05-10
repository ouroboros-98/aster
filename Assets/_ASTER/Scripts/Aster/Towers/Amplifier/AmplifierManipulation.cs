using Aster.Light;
using Aster.Utils;
using UnityEngine;

namespace Aster.Towers.Amplifier
{
    public class AmplifierManipulation : RayManipulation
    {
        private readonly Transform           emissionPoint;
        private readonly Transform           towerTransform;
        private readonly AmplifierParameters parameters;

        public AmplifierManipulation(AmplifierParameters parameters, Transform emissionPoint, Transform towerTransform)
        {
            this.parameters     = parameters;
            this.emissionPoint  = emissionPoint;
            this.towerTransform = towerTransform;
        }

        public override void Apply(LightHit hit, ILightRay rayIn, ILightRay rayOut)
        {
            rayOut.Origin      = emissionPoint.position.With(y: rayIn.Origin.y);
            rayOut.MaxDistance = parameters.NewMaxDistance;
            rayOut.Intensity   = rayIn.Intensity * parameters.IntensityScale;
            rayOut.Direction   = towerTransform.forward;
        }
    }
}