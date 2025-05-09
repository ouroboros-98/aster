using System;
using Aster.Core;
using Aster.Core.Entity;
using Aster.Core.Interactions.Grab;
using Aster.Light;
using DependencyInjection;
using UnityEngine.Serialization;

namespace Aster.Towers
{
    public abstract class BaseTower : BaseLightHittable, IDisableOnGrab
    {
        protected EntityHP HP;

        public abstract LightReceiver LightReceiver { get; }

        public bool Duplicated = false;

        protected void OnEnable()
        {
            if (LightReceiver != null) LightReceiver.Enabled = true;
        }

        protected void OnDisable()
        {
            if (LightReceiver != null) LightReceiver.Enabled = false;
        }

        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            LightReceiver.Register(lightHit);

            return CreateHitContext(lightHit);
        }

        protected virtual LightHitContext CreateHitContext(LightHit hit)
        {
            return new(hit, blockLight: true);
        }

        public override void OnLightRayExit(ILightRay ray)
        {
            LightReceiver.Deregister(ray);
        }

        protected override void Reset()
        {
            base.Reset();

            if (IsNotNull(Config)) AssignParametersFromConfig(Config);
        }

        protected virtual void AssignParametersFromConfig(AsterConfiguration config)
        {
        }
    }
}