using System;
using System.Collections.Generic;
using System.Linq;
using Aster.Light;
using UnityEngine;

namespace Aster.Towers
{
    public class SplitterManipulator : LightManipulator<List<SplittedRay>>
    {
        private readonly Splitter           _splitterTower;
        private readonly SplitterParameters _splitterParameters;

        private SplitterManipulation[] _splitterTransformations;

        public SplitterManipulator(Splitter splitter) : base(splitter)
        {
            _splitterTower      = splitter;
            _splitterParameters = splitter.Parameters;

            SetupTransformations();
        }

        private void SetupTransformations()
        {
            int count = _splitterParameters.SplitCount;

            _splitterTransformations = new SplitterManipulation[count];

            for (int i = 0; i < count; i++)
            {
                _splitterTransformations[i] = new SplitterManipulation(_splitterTower, i);
            }
        }

        protected override List<SplittedRay> CreateManipulation(LightHit lightHit)
        {
            return CreateDefaultList(lightHit);
        }

        protected override List<SplittedRay> UpdateManipulation(LightHit hit, List<SplittedRay> splittedRays)
        {
            splittedRays.ForEach(ray => ray.UpdateTransformation(hit));

            return splittedRays;
        }

        protected override void DestroyManipulation(ILightRay ray, List<SplittedRay> splitRays)
        {
            splitRays.ForEach(outRay => outRay.Destroy());
            splitRays.Clear();
        }

        private List<SplittedRay> CreateDefaultList(LightHit hit)
        {
            List<SplittedRay> result = new();

            for (int i = 0; i < _splitterParameters.SplitCount; i++)
            {
                SplittedRay splitRay = new(hit, _splitterTransformations[i]);
                splitRay.IgnoreHittable(_splitterTower);
                splitRay.ExistsWhen(() => hit.Ray != null && _splitterTower.LightReceiver.IsReceiving(hit.Ray));
                result.Add(splitRay);
            }

            return result;
        }
    }
}