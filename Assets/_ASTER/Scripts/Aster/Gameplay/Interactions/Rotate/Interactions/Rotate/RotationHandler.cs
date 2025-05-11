using System;
using Aster.Utils;
using NaughtyAttributes;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Core
{
    [System.Serializable]
    public class RotationHandler
    {
        private float INSTANT_ROTATION = 0f;
        private float MAX_STEP         = 360f;
        private float SPEED_SCALE      = 10f;

        #region FIELDS

        [SerializeField, Range(0, 200f), Tooltip("0 for instant rotation")]
        private float rotationSpeed = 2;

        public float RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
        }

        private Angle _currentAngle;
        private Angle _targetAngle;
        private Angle _activeTargetingAngle;

        private RotationData _currentRotation;
        private Transform    _transform;
        private bool         _instant = false;

        public bool           InvertDelta = false;
        public (Angle a, Angle b) Bounds      = (0f, 359.99999f);

        #endregion

        #region PROPERTIES

        public Angle CurrentAngle => _currentAngle;
        public Angle TargetAngle  => _targetAngle;

        public Angle ActiveTargetingAngle
        {
            get => _activeTargetingAngle;
            set => _activeTargetingAngle = value;
        }

        public bool IsRotating => _currentRotation is { IsFinished: false };

        #endregion

        #region EVENTS

        public event Action<float> OnTargetAngleSet;
        public event Action<float> OnAngleChange;
        public event Action<float> OnRotationFinish;

        #endregion

        public RotationHandler()
        {
            OnTargetAngleSet = delegate { };
            OnAngleChange    = delegate { };
            OnRotationFinish = delegate { };
        }

        public void Bind(Transform transform)
        {
            _transform    = transform;
            _currentAngle = _transform.rotation.eulerAngles.y;
            _targetAngle  = _transform.rotation.eulerAngles.y;
        }

        public void Rotate(Vector2 direction)
        {
            Rotate(direction.ToAngle());
        }

        public virtual void Rotate(Angle angle, bool instant = false)
        {
            if (Mathf.Approximately(angle, _targetAngle)) return;
            if(!angle.IsInBetween(Bounds.a,Bounds.b)) return;

            _instant     = instant;
            _targetAngle = angle;
            OnTargetAngleSet?.Invoke(_targetAngle);

            _currentRotation = (_currentAngle, _targetAngle);
        }

        public void RotateBy(Angle delta, bool instant = false)
        {
            if (Mathf.Approximately(delta, 0f)) return;

            if (InvertDelta) delta = -delta;

            Angle angle = _currentAngle + delta;
            Rotate(angle, instant);
        }

        public void Update()
        {
            if (!IsRotating) return;

            float step = (_instant || rotationSpeed == INSTANT_ROTATION)
                             ? MAX_STEP
                             : rotationSpeed * Time.deltaTime * SPEED_SCALE;
            Angle newAngle = _currentRotation.GetAngleAddProgress(step);

            UpdateRotation(newAngle);
        }

        private void UpdateRotation(Angle angle)
        {
            _currentAngle = angle;

            _transform.rotation = Quaternion.Euler(0f, angle, 0f);
            OnAngleChange?.Invoke(angle);

            if (_currentRotation.IsFinished)
            {
                _instant         = false;
                _currentRotation = null;
                OnRotationFinish?.Invoke(angle);
            }
        }
    }
}