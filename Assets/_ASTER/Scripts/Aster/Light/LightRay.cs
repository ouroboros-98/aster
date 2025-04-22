using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aster.Core;
using Aster.Entity;
using Aster.Entity.Enemy;
using Aster.Towers;
using Aster.Utils.Attributes;
using Aster.Utils.Pool;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Light
{
    [RequireComponent(typeof(LineRenderer))]
    public class LightRay : AsterMono, IPoolable
    {
        private const float MAX_DISTANCE = 100f;

        [SerializeField, BoxedProperty] private RayData _rayData = null;

        private ILightCaster _lightCaster;

        private Dictionary<BaseLightHittable, LightHitContext> rayHits = new();

        public RayData Data
        {
            get => _rayData;
            set
            {
                UnsubscribeRayEvents();
                _rayData = value;
                SubscribeRayEvents();
                if (_rayData != null) _lineRenderer.enabled = true;
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
            if (_rayData == null) return;

            _rayData.OriginChange   += OnOriginChanged;
            _rayData.EndPointChange += OnEndPointChanged;
            _rayData.WidthChange    += OnWidthChanged;
            _rayData.ColorChange    += OnColorChanged;
            _rayData.OnDestroy      += OnDestroy;

            _rayData.ForceUpdate();
        }

        private void UnsubscribeRayEvents()
        {
            if (_rayData == null) return;

            _rayData.OriginChange   -= OnOriginChanged;
            _rayData.EndPointChange -= OnEndPointChanged;
            _rayData.WidthChange    -= OnWidthChanged;
            _rayData.ColorChange    -= OnColorChanged;
            _rayData.OnDestroy      -= OnDestroy;
        }

        private void Awake()
        {
            ValidateComponent(ref _lineRenderer);

            _lightCaster = new LightRayCaster();
        }

        public void Reset()
        {
            Hittable?.OnLightRayExit(this);

            Data = null;

            _lineRenderer.enabled = false;
            Hittable              = null;

            rayHits.Clear();
        }

        private void FixedUpdate()
        {
            if (!Data.CheckExists()) return;

            List<LightHit> hits = _lightCaster.GetHits(Data);

            HashSet<BaseLightHittable> hittables = new();

            if (hits.Count == 0)
            {
                Data.EndPoint = Data.Origin + Data.Direction * RayData.MAX_DISTANCE;
            }

            foreach (var hit in hits)
            {
                if (Data.CheckIgnoreHittable(hit.Hittable)) continue;

                var hitContext = hit.Hittable.OnLightRayHit(new(this, hit.HitPoint, hit.Hittable));

                hittables.Add(hit.Hittable);

                if (!HandleHit(hitContext)) break;
            }

            RefreshHittables(hittables);
        }

        private void RefreshHittables(HashSet<BaseLightHittable> hittables)
        {
            HashSet<BaseLightHittable> allHittables = new(rayHits.Keys);

            HashSet<BaseLightHittable> toRemove = new(allHittables);
            toRemove.ExceptWith(hittables);

            foreach (var hittable in toRemove)
            {
                debugPrint($"Removing {hittable.gameObject.name}");

                hittable.OnLightRayExit(this);
                rayHits.Remove(hittable);
            }

            debugPrint($"Hittables: {string.Join(", ", hittables.Select(h => h.gameObject.name))}");
        }

        private bool HandleHit(LightHitContext hitContext)
        {
            rayHits[hitContext.Hit.Hittable] = hitContext;

            return !hitContext.BlockLight;
        }

        private RaycastHit[] GetHits()
        {
            return Physics.RaycastAll(Data.Origin, Data.Direction, RayData.MAX_DISTANCE);
        }

        void OnOriginChanged(Vector3   value) => _lineRenderer?.SetPosition(0, value);
        void OnEndPointChanged(Vector3 value) => _lineRenderer?.SetPosition(1, value);
        void OnWidthChanged(float      width) => _lineRenderer.startWidth = _lineRenderer.endWidth = width;
        void OnColorChanged(Color      color) => _lineRenderer.startColor = _lineRenderer.endColor = color;

        private void OnDestroy()
        {
            _lineRenderer.enabled = false;
            rayHits.Clear();

            RayPool.Instance.Return(this);
        }

        public static implicit operator RayData(LightRay lightRay) => lightRay._rayData;
    }
}