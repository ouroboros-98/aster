using Aster.Utils.Attributes;
using Aster.Utils.Pool;
using JetBrains.Annotations;
using UnityEngine;

namespace Aster.Towers
{
    public class Splitter : BaseTower
    {
        [SerializeField, BoxedProperty] private SplitterParameters _splitterParameters = new(4, 45, .1f);

        private         LightReceiver _lightReceiver;
        public override LightReceiver LightReceiver => _lightReceiver;

        private SplitterParameters  lastParameterState;
        private SplitterManipulator splitterManipulator;

        public SplitterParameters Parameters => _splitterParameters;

        private void Awake()
        {
            _lightReceiver = new LightReceiver();
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
}