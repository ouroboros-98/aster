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

        private RayHitTracker _rayHitTracker;

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
                if (lightRay != null)
                {
                    _lineRenderer.enabled = true;
                    _rayHitTracker        = new RayHitTracker(lightRay);
                }
            }
        }

        private LineRenderer _lineRenderer;

        private Vector3 _creatorInitialPosition;
        private Vector3 _creatorInitialDirection;


        private void OnEnable() => SubscribeRayEvents();

        private void OnDisable()
        {
            UnsubscribeRayEvents();
            _rayHitTracker?.RefreshHittables(new HashSet<BaseLightHittable>());
        }

        private void SubscribeRayEvents()
        {
            if (lightRay == null) return;

            lightRay.OriginChange    += OnOriginChanged;
            lightRay.EndPointChange  += OnEndPointChanged;
            lightRay.IntensityChange += OnIntensityChanged;
            lightRay.WidthChange     += OnWidthChanged;

            if (lightRay is not TargetingRay)
            {
                lightRay.ColorChange += OnColorChanged;
            }
            else
            {
                OnColorChanged(Config.Targeting.RayColor);
            }

            lightRay.OnDestroy += OnRayDestroyed;

            lightRay.ForceUpdate();
        }

        private void UnsubscribeRayEvents()
        {
            if (lightRay == null) return;

            lightRay.OriginChange    -= OnOriginChanged;
            lightRay.EndPointChange  -= OnEndPointChanged;
            lightRay.IntensityChange -= OnIntensityChanged;
            lightRay.WidthChange     -= OnWidthChanged;
            lightRay.ColorChange     -= OnColorChanged;
            lightRay.OnDestroy       -= OnRayDestroyed;
        }

        private void Awake()
        {
            ValidateComponent(ref _lineRenderer);

            _lightCaster = new LightSphereCaster();
        }

        public void Reset()
        {
            Data = null;

            _lineRenderer.enabled = false;

            transform.position = Vector3.zero;
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

                if (!_rayHitTracker.HandleHit(hitContext)) break;
            }

            _rayHitTracker.RefreshHittables(hittables);

            RayLogger.Update();
        }


        void OnOriginChanged(Vector3 value) => _lineRenderer?.SetPosition(0, value);

        void OnEndPointChanged(Vector3 value)
        {
            _lineRenderer?.SetPosition(1, value);
        }

        void OnWidthChanged(float width) => _lineRenderer.startWidth = _lineRenderer.endWidth = width;

        void OnColorChanged(Color color)
        {
            Color matColor = _lineRenderer.material.GetColor("_Color");
            _lineRenderer.material.SetColor("_Color", new(color.r, color.g, color.b, matColor.a));
        }

        void OnIntensityChanged(float intensity)
        {
            Color color = _lineRenderer.material.GetColor("_Color");
            _lineRenderer.material.SetColor("_Color", new(color.r, color.g, color.b, MathF.Pow(intensity, 1 / 2f)));
        }

        private void OnRayDestroyed()
        {
            _lineRenderer.enabled = false;
            RayLogger.LightRay    = null;

            RayPool.Instance.Return(this);
        }
    }
}