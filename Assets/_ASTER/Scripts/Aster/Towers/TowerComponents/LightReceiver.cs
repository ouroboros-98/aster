using System;
using System.Collections.Generic;
using System.Linq;
using Aster.Light;

namespace Aster.Towers
{
    public class LightReceiver
    {
        private bool targetOnlyMode = false;

        public bool TargetOnlyMode
        {
            get => targetOnlyMode;
            set
            {
                if (value == targetOnlyMode) return;
                targetOnlyMode = value;

                _lightHits.Keys.ToList().ForEach(ray => Deregister(ray));
                _lightHits.Clear();
            }
        }

        private Dictionary<ILightRay, LightHit> _lightHits;

        public event Action<LightHit>  Entry        = delegate { };
        public event Action<LightHit>  Update       = delegate { };
        public event Action<ILightRay> OnDeregister = delegate { };


        public LightReceiver()
        {
            _lightHits = new();
        }

        public void Register(LightHit hit)
        {
            if (!ShouldAcceptHit(hit)) return;
            bool isNew = !_lightHits.ContainsKey(hit.Ray);
            _lightHits[hit.Ray] = hit;

            if (isNew) Entry?.Invoke(hit);

            Update?.Invoke(hit);
        }

        protected virtual bool ShouldAcceptHit(LightHit hit)
        {
            return true;
        }

        public bool IsReceiving(ILightRay ray)
        {
            return _lightHits.ContainsKey(ray);
        }

        public LightHit this[ILightRay ray]
        {
            get
            {
                if (_lightHits.TryGetValue(ray, out var hit))
                    return hit;

                throw new KeyNotFoundException($"No light hit found for ray: {ray}");
            }
        }

        public void Deregister(ILightRay data)
        {
            _lightHits.Remove(data);
            OnDeregister?.Invoke(data);
        }

        public void ForEach(Action<LightHit> action)
        {
            foreach (var hit in _lightHits.Values)
            {
                action?.Invoke(hit);
            }
        }
    }
}