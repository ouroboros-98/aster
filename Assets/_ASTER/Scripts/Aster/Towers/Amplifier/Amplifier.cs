using Aster.Utils.Attributes;
using UnityEngine;

namespace Aster.Towers.Amplifier
{
    public class Amplifier : BaseTower
    {
        [SerializeField]                private Transform           emissionPoint;
        [SerializeField, BoxedProperty] private AmplifierParameters parameters;

        private AmplifierManipulation _manipulation;
        private RayManipulator        _rayManipulator;

        private         LightReceiver _lightReceiver;
        public override LightReceiver LightReceiver => _lightReceiver;

        protected override void Awake()
        {
            base.Awake();

            _lightReceiver  = new LightReceiver();
            _manipulation   = new AmplifierManipulation(parameters, emissionPoint, transform);
            _rayManipulator = new RayManipulator(_lightReceiver, transform, _manipulation);
        }
    }
}