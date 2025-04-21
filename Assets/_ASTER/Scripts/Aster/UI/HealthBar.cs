using System;
using Aster.Core.Entity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace Aster.Core.UI
{
    public class HealthBar : AsterMono
    {
        private LookAtConstraint _lookAt;

        [ShowNonSerializedField] private Transform _cameraAimPoint;

        [SerializeField] private Image _fill;
        [SerializeField] private Image _background;

        private void Awake()
        {
            Reset();

            _fill.fillAmount = 1;

            if (_cameraAimPoint == null) _cameraAimPoint = AsterCamera.Instance.AimPoint;

            _lookAt.AddSource(
                              new ConstraintSource
                              {
                                  sourceTransform = _cameraAimPoint,
                                  weight          = 1
                              }
                             );
        }

        public void OnHPChanged(EntityHP.HPChangeContext context)
        {
            if (_fill == null) return;

            float fillAmount = (float)context.CurrentValue / context.MaxValue;
            _fill.fillAmount = fillAmount;
        }

        private void Reset()
        {
            ValidateComponent(ref _lookAt);
        }
    }
}