using System;
using System.Collections.Generic;
using Aster.Light;

namespace Aster.Towers
{
    public class LightReceiver
    {
        private Dictionary<RayData, LightHit> _lightHits;

        public event Action<LightHit> Entry        = delegate { };
        public event Action<LightHit> Update       = delegate { };
        public event Action<RayData>  OnDeregister = delegate { };


        public LightReceiver()
        {
            _lightHits = new();
        }

        public void Register(LightHit hit)
        {
            bool isNew = !_lightHits.ContainsKey(hit.Ray);
            _lightHits[hit.Ray] = hit;

            if (isNew) Entry?.Invoke(hit);

            Update?.Invoke(hit);
        }

        public bool IsReceiving(RayData ray)
        {
            return _lightHits.ContainsKey(ray);
        }

        public LightHit this[RayData ray]
        {
            get
            {
                if (_lightHits.TryGetValue(ray, out var hit))
                    return hit;

                throw new KeyNotFoundException($"No light hit found for ray: {ray}");
            }
        }

        public void Deregister(RayData data)
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