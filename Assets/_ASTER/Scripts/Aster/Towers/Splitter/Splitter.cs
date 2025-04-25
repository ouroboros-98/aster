using Aster.Light;
using Aster.Utils.Attributes;
using Aster.Utils.Pool;
using JetBrains.Annotations;
using UnityEngine;

namespace Aster.Towers
{
    public class Splitter : BaseTower
    {
        [SerializeField, BoxedProperty] private SplitterParameters _splitterParameters = new(4, 45, .1f, false, 0);

        private         LightReceiver _lightReceiver;
        public override LightReceiver LightReceiver => _lightReceiver;

        private SplitterParameters  lastParameterState;
        private SplitterManipulator splitterManipulator;

        public SplitterParameters Parameters => _splitterParameters;

        protected override void Awake()
        {
            base.Awake();

            _lightReceiver = new LightReceiver();
            OnUpdateParameters();
        }

        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            if (Parameters.Refract && (lightHit.Ray.Color != Color.white))
            {
                return new(lightHit, blockLight: true);
            }

            return base.OnLightRayHit(lightHit);
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
}