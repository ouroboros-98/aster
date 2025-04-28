using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Towers.Destabilizer
{
    public class Destabilizer : BaseTower
    {
        [ShowNonSerializedField, BoxedProperty] private DestabilizerParameters _parameters = new();

        private         LightReceiver  _lightReceiver;
        public override LightReceiver  LightReceiver => _lightReceiver;
        private         RayManipulator _manipulator;

        private DestabilizerManipulation _manipulation;

        protected override void Awake()
        {
            _lightReceiver = new LightReceiver();
            _manipulation  = new DestabilizerManipulation(_parameters, transform);
            _manipulator   = new RayManipulator(_lightReceiver, transform, _manipulation);
        }

        protected override void Reset()
        {
            base.Reset();

            DestabilizerParameters configParameters   = Configuration.Towers.Destabilizer;
            if (configParameters != null) _parameters = configParameters;
        }
    }
}