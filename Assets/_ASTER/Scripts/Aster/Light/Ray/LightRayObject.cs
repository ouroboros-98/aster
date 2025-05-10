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
        private const float WIDTH_SCALE = 3f;

        private static readonly int   LineColorIndex = Shader.PropertyToID("_Color");
        private                 Color LineColor => _lineRenderer.material.GetColor(LineColorIndex);

        private ILightRay lightRay = null;

        [SerializeField, BoxedProperty]
        LightRayLogger RayLogger = new();

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

        private LineRenderer _lineRenderer;

        private void OnEnable()  => SubscribeRayEvents();
        private void OnDisable() => UnsubscribeRayEvents();

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
        }

        public void Reset()
        {
            Data = null;

            _lineRenderer.enabled = false;
            transform.position    = Vector3.zero;
        }

        private void FixedUpdate()
        {
            RayLogger.Update();
        }


        void OnOriginChanged(Vector3 value) => _lineRenderer?.SetPosition(0, value);

        void OnEndPointChanged(Vector3 value)
        {
            _lineRenderer?.SetPosition(1, value);
        }

        void OnWidthChanged(float width) => _lineRenderer.startWidth = _lineRenderer.endWidth = width * WIDTH_SCALE;

        void OnColorChanged(Color color) =>
            _lineRenderer.material.SetColor(LineColorIndex, new(color.r, color.g, color.b, LineColor.a));

        void OnIntensityChanged(float intensity) =>
            _lineRenderer.material.SetColor(LineColorIndex,
                                            new(LineColor.r, LineColor.g, LineColor.b, MathF.Pow(intensity, 1 / 2f)));

        private void OnRayDestroyed()
        {
            _lineRenderer.enabled = false;
            RayLogger.LightRay    = null;

            RayPool.Instance.Return(this);
        }
    }
}