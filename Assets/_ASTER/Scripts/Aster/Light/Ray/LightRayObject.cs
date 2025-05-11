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

        [SerializeField]
        private Transform[] originBoundedObjects;

        [SerializeField]
        private Transform[] endpointBoundedObjects;

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
        private float        baseAlpha = 1;

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


        void OnOriginChanged(Vector3 value)
        {
            _lineRenderer?.SetPosition(0, value);

            if (originBoundedObjects == null) return;
            foreach (Transform t in originBoundedObjects) t.position = value;
        }

        void OnEndPointChanged(Vector3 value)
        {
            bool isMaxDistance = Vector3.Distance(value, Data.Origin) + .05f >= Data.MaxDistance;

            float    endAlpha = !isMaxDistance ? 1 : 0;
            Gradient gradient = new();
            gradient.SetKeys(
                             new[] { new GradientColorKey(Color.white, 0) },
                             new[]
                             {
                                 new GradientAlphaKey(1,        .75f),
                                 new GradientAlphaKey(endAlpha, 1)
                             }
                            );
            _lineRenderer.colorGradient = gradient;

            Vector3 actualValue = !isMaxDistance ? value : value + Data.Direction * Data.MaxDistance * .1f;

            _lineRenderer?.SetPosition(1, actualValue);
            _lineRenderer?.SetPosition(1, value);

            if (endpointBoundedObjects == null) return;
            foreach (Transform t in endpointBoundedObjects) t.position = value;
        }

        void OnWidthChanged(float width) => _lineRenderer.startWidth = _lineRenderer.endWidth = width * WIDTH_SCALE;

        void OnColorChanged(Color color) =>
            _lineRenderer.material.SetColor(LineColorIndex, new(color.r, color.g, color.b, baseAlpha));

        void OnIntensityChanged(float intensity)
        {
            baseAlpha = MathF.Pow(intensity, 1 / 2f);
            _lineRenderer.material.SetColor(LineColorIndex,
                                            new(LineColor.r, LineColor.g, LineColor.b, baseAlpha));
        }

        private void OnRayDestroyed()
        {
            _lineRenderer.enabled = false;
            RayLogger.LightRay    = null;

            RayPool.Instance.Return(this);
        }
    }
}