using System;
using Aster.Core.Entity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace Aster.Core.UI
{
    public class PopUpIndicator : AsterMono
    {
        private LookAtConstraint _lookAt;

        [ShowNonSerializedField] private Transform _cameraAimPoint;

        [SerializeField] private Image indicatorImage;

        private void Awake()
        {
            Reset();
            

            if (_cameraAimPoint == null) _cameraAimPoint = AsterCamera.Instance.AimPoint;

            _lookAt.AddSource(
                new ConstraintSource
                {
                    sourceTransform = _cameraAimPoint,
                    weight          = 1
                }
            );
        }
        

        private void Reset()
        {
            ValidateComponent(ref _lookAt);
        }
    }
}