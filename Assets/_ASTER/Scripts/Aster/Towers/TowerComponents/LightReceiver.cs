using System;
using System.Collections.Generic;
using System.Linq;
using Aster.Light;
using Unity.VisualScripting;

namespace Aster.Towers
{
    public class LightReceiver
    {
        private bool enabled = true;

        public bool Enabled
        {
            get => enabled;
            set
            {
                if (value == enabled) return;
                enabled = value;
                if (enabled) OnEnable();
                else OnDisable();
            }
        }

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

        public int Count => _lightHits.Count;

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
            if (!enabled) return;
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

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
            ForEach(hit => { hit.Ray.Destroy(); });
            _lightHits.Clear();
        }

        public void ForEach(Action<LightHit> action)
        {
            Dictionary<ILightRay, LightHit>.ValueCollection hits = _lightHits.Values;

            foreach (var hit in hits)
            {
                action?.Invoke(hit);
            }
        }
    }
}