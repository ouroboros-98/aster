using Aster.Core;
using Aster.Light;
using Aster.Utils.Attributes;
using Aster.Utils.Pool;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Towers
{
    public class Splitter : BaseTower
    {
        [ShowNonSerializedField, BoxedProperty] protected SplitterParameters _splitterParameters;

        private         LightReceiver _lightReceiver;
        public override LightReceiver LightReceiver => _lightReceiver;

        private SplitterParameters  lastParameterState;
        private SplitterManipulator splitterManipulator;

        public SplitterParameters Parameters => _splitterParameters;

        protected override void Awake()
        {
            base.Awake();

            _lightReceiver      = new LightReceiver();
            splitterManipulator = new(this);
        }

        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            if (Parameters.Refract && (lightHit.Ray.Color != Color.white))
            {
                return new(lightHit, blockLight: true);
            }

            return base.OnLightRayHit(lightHit);
        }

        protected override void AssignParametersFromConfig(AsterConfiguration config)
        {
            SplitterParameters configParameters                  = config.Towers.Splitter;
            if (IsNotNull(configParameters)) _splitterParameters = configParameters;
        }
    }
}