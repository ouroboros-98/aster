using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aster.Core;
using Aster.Entity;
using Aster.Entity.Enemy;
using Aster.Towers;
using Aster.Utils;
using Aster.Utils.Attributes;
using Aster.Utils.Pool;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Light
{
    [RequireComponent(typeof(LineRenderer))]
    public class LightRayObject : AsterMono, IPoolable
    {
        private const float MAX_DISTANCE = 100f;

        [FormerlySerializedAs("_rayData")] [SerializeField, BoxedProperty] private ILightRay lightRay = null;

        private ILightCaster _lightCaster;

        private Dictionary<BaseLightHittable, LightHitContext> rayHits = new();

        [SerializeField, BoxedProperty] LightRayLogger RayLogger = new();

        public ILightRay Data
        {
            get => lightRay;
            set
            {
                UnsubscribeRayEvents();
                lightRay           = value;
                RayLogger.LightRay = lightRay;
                SubscribeRayEvents();
                if (lightRay != null) _lineRenderer.enabled = true;
            }
        }

        private BaseLightHittable _hittable;

        private BaseLightHittable Hittable
        {
            get => _hittable;
            set
            {
                if (_hittable != null && _hittable == value) return;
                _hittable?.OnLightRayExit(this);
                _hittable = value;
            }
        }

        private LineRenderer _lineRenderer;

        private Vector3 _creatorInitialPosition;
        private Vector3 _creatorInitialDirection;


        private void OnEnable() => SubscribeRayEvents();

        private void OnDisable()
        {
            UnsubscribeRayEvents();
            RefreshHittables(new HashSet<BaseLightHittable>());
        }

        private void SubscribeRayEvents()
        {
            if (lightRay == null) return;

            lightRay.OriginChange   += OnOriginChanged;
            lightRay.EndPointChange += OnEndPointChanged;
            lightRay.WidthChange    += OnWidthChanged;

            if (lightRay is not TargetingRay)
            {
                lightRay.ColorChange += OnColorChanged;
            }
            else
            {
                OnColorChanged(TargetingRay.TARGETING_RAY_COLOR);
            }

            lightRay.OnDestroy += OnRayDestroyed;

            lightRay.ForceUpdate();
        }

        private void UnsubscribeRayEvents()
        {
            if (lightRay == null) return;

            lightRay.OriginChange   -= OnOriginChanged;
            lightRay.EndPointChange -= OnEndPointChanged;
            lightRay.WidthChange    -= OnWidthChanged;
            lightRay.ColorChange    -= OnColorChanged;
            lightRay.OnDestroy      -= OnRayDestroyed;
        }

        private void Awake()
        {
            ValidateComponent(ref _lineRenderer);

            _lightCaster = new LightSphereCaster();
        }

        public void Reset()
        {
            Hittable?.OnLightRayExit(this);

            Data = null;

            _lineRenderer.enabled = false;
            Hittable              = null;

            transform.position = Vector3.zero;

            rayHits.Clear();
        }

        private void FixedUpdate()
        {
            if (!Data.CheckExists()) return;

            List<LightHit> hits = _lightCaster.GetHits(Data);

            HashSet<BaseLightHittable> hittables = new();

            if (hits.Count == 0)
            {
                Data.EndPoint = Data.Origin + Data.Direction * LightRay.MAX_DISTANCE;
            }

            foreach (var hit in hits)
            {
                if (Data.CheckIgnoreHittable(hit.Hittable)) continue;

                var hitContext = hit.Hittable.LightHit(new(this.lightRay, hit.HitPoint, hit.Hittable));

                hittables.Add(hit.Hittable);

                if (!HandleHit(hitContext)) break;
            }

            RefreshHittables(hittables);

            RayLogger.Update();
        }

        private void RefreshHittables(HashSet<BaseLightHittable> hittables)
        {
            HashSet<BaseLightHittable> allHittables = new(rayHits.Keys);

            HashSet<BaseLightHittable> toRemove = new(allHittables);
            toRemove.ExceptWith(hittables);

            foreach (var hittable in toRemove)
            {
                hittable.OnLightRayExit(this);
                rayHits.Remove(hittable);
            }
        }

        private bool HandleHit(LightHitContext hitContext)
        {
            rayHits[hitContext.Hit.Hittable] = hitContext;

            return !hitContext.BlockLight;
        }

        void OnOriginChanged(Vector3 value) => _lineRenderer?.SetPosition(0, value);

        void OnEndPointChanged(Vector3 value)
        {
            _lineRenderer?.SetPosition(1, value);
        }

        void OnWidthChanged(float width) => _lineRenderer.startWidth = _lineRenderer.endWidth = width;

        void OnColorChanged(Color color)
        {
            // _lineRenderer.startColor = _lineRenderer.endColor = color;
            _lineRenderer.material.SetColor("_Color", new(color.r, color.g, color.b, Data.Intensity));
        }

        void OnIntensityChanged(float intensity)
        {
            OnColorChanged(Data.Color);
        }

        private void OnRayDestroyed()
        {
            _lineRenderer.enabled = false;
            rayHits.Clear();
            RayLogger.LightRay = null;

            RayPool.Instance.Return(this);
        }
    }
}