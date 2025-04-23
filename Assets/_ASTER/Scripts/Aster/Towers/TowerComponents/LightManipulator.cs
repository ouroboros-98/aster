using System.Collections.Generic;
using Aster.Light;
using UnityEngine;

namespace Aster.Towers
{
    public abstract class LightManipulator<T>
    {
        protected readonly LightReceiver LightReceiver;
        protected readonly Transform     Transform;

        private Dictionary<LightRay, T> _manipulations;

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

        private void OnDeregister(LightRay ray)
        {
            if (!_manipulations.ContainsKey(ray)) return;

            T manipulation = _manipulations[ray];
            if (manipulation != null) DestroyManipulation(ray, manipulation);

            _manipulations.Remove(ray);
        }

        protected abstract T    CreateManipulation(LightHit lightHit);
        protected abstract T    UpdateManipulation(LightHit hit, T manipulation);
        protected abstract void DestroyManipulation(LightRay ray, T manipulation);
    }
}