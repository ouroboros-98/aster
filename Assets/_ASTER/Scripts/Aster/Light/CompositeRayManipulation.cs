using System.Collections.Generic;

namespace Aster.Light
{
    public class CompositeRayManipulation : RayManipulation
    {
        private readonly List<RayManipulation> _manipulations;

        public CompositeRayManipulation()
        {
            _manipulations = new List<RayManipulation>();
        }

        public CompositeRayManipulation(params RayManipulation[] manipulations)
        {
            _manipulations = new List<RayManipulation>(manipulations);
        }

        public CompositeRayManipulation(CompositeRayManipulation compositeRayManipulation)
        {
            _manipulations = new(compositeRayManipulation._manipulations);
        }

        public override void Apply(LightHit hit, LightRay rayIn, LightRay rayOut)
        {
            if (_manipulations.Count == 0) return;

            LightRay ray = new LightRay(rayIn, false);

            foreach (RayManipulation manipulation in _manipulations)
            {
                ray = manipulation.GetManipulatedRay(hit, ray);
            }

            rayOut.Set(ray);
        }

        public sealed override CompositeRayManipulation Append(RayManipulation manipulation)
        {
            _manipulations.Add(manipulation);
            return this;
        }
    }
}