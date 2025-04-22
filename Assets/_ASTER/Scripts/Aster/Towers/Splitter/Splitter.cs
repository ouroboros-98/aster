using System;
using Aster.Light;
using Aster.Utils;
using Aster.Utils.Attributes;
using Aster.Utils.Pool;
using JetBrains.Annotations;
using UnityEngine;

namespace Aster.Towers
{
    public class Splitter : BaseTower
    {
        [SerializeField, BoxedProperty] private SplitterParameters _splitterParameters = new(4, 45, .1f);

        private SplitterParameters  lastParameterState;
        private SplitterManipulator splitterManipulator;

        public SplitterParameters Parameters => _splitterParameters;

        private void Awake()
        {
            OnUpdateParameters();
        }

        private void Start()
        {
            lastParameterState = _splitterParameters;
        }

        private void Update()
        {
            if (_splitterParameters == lastParameterState) return;

            lastParameterState = _splitterParameters;
            OnUpdateParameters();
        }

        void OnUpdateParameters()
        {
            splitterManipulator?.Unbind();
            splitterManipulator = new(this);
        }
    }

    public class SplitLightData : IDisposable
    {
        private readonly float    _angleOffset;
        private readonly float    _originOffset;
        private readonly Splitter _splitterTower;
        private readonly RayData  _lightRay;

        public SplitLightData(LightHit hit, float angleOffset, float originOffset, Splitter splitterTower)
        {
            _angleOffset   = angleOffset;
            _originOffset  = originOffset;
            _splitterTower = splitterTower;

            _lightRay = CreateRay(hit);
        }

        private RayData CreateRay(LightHit hit)
        {
            Vector3 newDir = CalculateDirection(hit);
            Vector3 origin = CalculateOrigin(hit, newDir);

            RayData rayData = hit.Ray.ContinueRay(origin: origin, direction: newDir);
            rayData.IgnoreHittable(_splitterTower);
            rayData.ExistsWhen(() => hit.Ray != null && _splitterTower.LightReceiver.IsReceiving(hit.Ray));

            return rayData;
        }

        private Vector3 CalculateOrigin(LightHit hit, Vector3 newDir) =>
            hit.HitPoint + newDir.normalized * _originOffset;

        private Vector3 CalculateDirection(LightHit hit) =>
            Quaternion.AngleAxis(_angleOffset, Vector3.up) * hit.Ray.Direction;

        public void Update(LightHit hit)
        {
            _lightRay.Direction = CalculateDirection(hit);
            _lightRay.Origin    = CalculateOrigin(hit, _lightRay.Direction);
        }

        public void Destroy()
        {
            _lightRay.Destroy();
        }

        public void Dispose()
        {
            Destroy();
        }
    }
}