using System.Collections.Generic;
using System.Linq;
using Aster.Light;
using UnityEngine;

namespace Aster.Towers
{
    public class RayManipulator
    {
        protected readonly LightReceiver   LightReceiver;
        protected readonly Transform       Transform;
        protected readonly RayManipulation RayManipulation;

        private Dictionary<ILightRay, ILightRay> _manipulations;

        public RayManipulator(LightReceiver lightReceiver, Transform transform, RayManipulation rayManipulation)
        {
            _manipulations = new();

            LightReceiver   = lightReceiver;
            Transform       = transform;
            RayManipulation = rayManipulation;

            lightReceiver.Entry        += Tick;
            lightReceiver.Update       += Tick;
            lightReceiver.OnDeregister += OnRayLeave;
        }

        public void Tick(LightHit hit)
        {
            if (!_manipulations.ContainsKey(hit.Ray))
            {
                _manipulations[hit.Ray] = CreateManipulation(hit);
            }

            RayManipulation.Apply(hit, _manipulations[hit.Ray]);
        }

        public ILightRay CreateManipulation(LightHit lightHit)
        {
            ILightRay inRay  = lightHit.Ray;
            ILightRay newRay = inRay.Clone(false);

            newRay.ExistsWhen(() => inRay.CheckExists());
            newRay.ExistsWhen(() => LightReceiver != null && LightReceiver.IsReceiving(inRay));

            newRay.Activate();

            return newRay;
        }

        public void OnRayLeave(ILightRay ray)
        {
            if (!_manipulations.ContainsKey(ray)) return;

            _manipulations[ray]?.Destroy();
            _manipulations.Remove(ray);
        }
    }

    public abstract class LightManipulator<T>
    {
        protected readonly LightReceiver LightReceiver;
        protected readonly Transform     Transform;

        private Dictionary<ILightRay, T> _manipulations;

        public LightManipulator(BaseTower tower)
        {
            _manipulations = new();

            LightReceiver = tower.LightReceiver;
            Transform     = tower.transform;

            Bind();
        }

        private void Bind()
        {
            LightReceiver.Entry        += OnEntry;
            LightReceiver.Update       += OnUpdate;
            LightReceiver.OnDeregister += OnDeregister;

            LightReceiver.ForEach(OnEntry);
        }

        public void Unbind()
        {
            LightReceiver.ForEach((hit) => OnDeregister(hit.Ray));

            LightReceiver.Entry        -= OnEntry;
            LightReceiver.Update       -= OnUpdate;
            LightReceiver.OnDeregister -= OnDeregister;
        }

        private void OnEntry(LightHit hit)
        {
            _manipulations[hit.Ray] = CreateManipulation(hit);
        }

        private void OnUpdate(LightHit hit)
        {
            T manipulation = _manipulations[hit.Ray];

            if (manipulation == null) return;

            manipulation = UpdateManipulation(hit, manipulation);
        }

        private void OnDeregister(ILightRay ray)
        {
            if (!_manipulations.ContainsKey(ray)) return;

            T manipulation = _manipulations[ray];
            if (manipulation != null) DestroyManipulation(ray, manipulation);

            _manipulations.Remove(ray);
        }

        protected abstract T    CreateManipulation(LightHit   lightHit);
        protected abstract T    UpdateManipulation(LightHit   hit, T manipulation);
        protected abstract void DestroyManipulation(ILightRay ray, T manipulation);
    }
}