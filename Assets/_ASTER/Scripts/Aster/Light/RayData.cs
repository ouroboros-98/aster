using System;
using System.Collections.Generic;
using Aster.Core;
using Aster.Utils;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Light
{
    [System.Serializable]
    public class RayData
    {
        public static readonly Color DEFAULT_COLOR = Color.white;

        public const float MAX_DISTANCE      = 100f;
        public const float DEFAULT_INTENSITY = 1f;
        public const float DEFAULT_WIDTH     = .05f;

        [OnValueChanged("Editor__OnOriginChanged")] [SerializeField] private Vector3 _origin = Vector3.zero;

        [OnValueChanged("Editor__OnEndPointChanged")] [SerializeField] private Vector3 _endPoint = Vector3.forward;

        [OnValueChanged("Editor__OnIntensityChanged")] [SerializeField] private float _intensity = DEFAULT_INTENSITY;

        [OnValueChanged("Editor__OnWidthChanged")] [SerializeField] private float _width = DEFAULT_WIDTH;

        [OnValueChanged("Editor__OnColorChanged")] [SerializeField] private Color _color = DEFAULT_COLOR;

        [OnValueChanged("Editor_OnDirectionChanged")] [SerializeField]
        private Vector3Normalized _direction = Vector3.forward;

        [SerializeField] private List<BaseLightHittable> ignoreHittables;

        private List<Func<bool>>          existencePredicates;
        public  IReadOnlyList<Func<bool>> ExistencePredicates => existencePredicates;

        public bool IsActive { get; private set; }


#if UNITY_EDITOR
        private void Editor__OnOriginChanged()    => OriginChange?.Invoke(_origin);
        private void Editor__OnEndPointChanged()  => EndPointChange?.Invoke(_endPoint);
        private void Editor__OnIntensityChanged() => IntensityChange?.Invoke(_intensity);
        private void Editor__OnWidthChanged()     => WidthChange?.Invoke(_width);
        private void Editor__OnColorChanged()     => ColorChange?.Invoke(_color);
        private void Editor__OnDirectionChanged() => DirectionChange?.Invoke(_direction);
#endif

        public Vector3 Origin
        {
            get => _origin;
            set
            {
                if (_origin == value) return;
                _origin = value;
                OriginChange?.Invoke(_origin);
            }
        }

        public Vector3 EndPoint
        {
            get => _endPoint;
            set
            {
                if (_endPoint == value) return;
                _endPoint = value;
                EndPointChange?.Invoke(_endPoint);
            }
        }

        public float Intensity
        {
            get => _intensity;
            set
            {
                if (Mathf.Approximately(_intensity, value)) return;
                _intensity = value;
                IntensityChange?.Invoke(_intensity);
            }
        }

        public float Width
        {
            get => _width;
            set
            {
                if (Mathf.Approximately(_width, value)) return;
                _width = value;
                WidthChange?.Invoke(_width);
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                if (_color == value) return;
                _color = value;
                ColorChange?.Invoke(_color);
            }
        }

        public Vector3Normalized Direction
        {
            get => _direction;
            set
            {
                if (_direction == value) return;

                DirectionChange?.Invoke(value);
                _direction = value;
            }
        }

        public event Action<Vector3>           OriginChange    = delegate { };
        public event Action<Vector3>           EndPointChange  = delegate { };
        public event Action<float>             IntensityChange = delegate { };
        public event Action<float>             WidthChange     = delegate { };
        public event Action<Color>             ColorChange     = delegate { };
        public event Action<Vector3Normalized> DirectionChange = delegate { };

        public event Action OnDestroy = delegate { };

        private void OnDirectionChange(Vector3Normalized direction) => EndPoint = _origin + direction * MAX_DISTANCE;

        public RayData()
        {
            IsActive = true;

            DirectionChange += OnDirectionChange;

            existencePredicates = new();
            ignoreHittables     = new();

            AsterEvents.Instance.OnRayCreated?.Invoke(this);
        }

        public RayData(RayData source) : base()
        {
            IsActive = true;

            DirectionChange += OnDirectionChange;

            _origin    = source._origin;
            _endPoint  = source._endPoint;
            _direction = source._direction;
            _intensity = source._intensity;
            _width     = source._width;
            _color     = source._color;

            existencePredicates = new();
            ignoreHittables     = new();

            AsterEvents.Instance.OnRayCreated?.Invoke(this);
        }

        private void RecalculateDirection() => _direction = Direction;

        public void Destroy()
        {
            IsActive = false;
            OnDestroy?.Invoke();
        }

        public void ForceUpdate()
        {
            OriginChange?.Invoke(_origin);
            EndPointChange?.Invoke(_endPoint);
            IntensityChange?.Invoke(_intensity);
            WidthChange?.Invoke(_width);
            ColorChange?.Invoke(_color);
        }

        public void IgnoreHittable(BaseLightHittable hittable)
        {
            if (ignoreHittables.Contains(hittable)) return;
            ignoreHittables.Add(hittable);
        }

        public bool CheckIgnoreHittable(BaseLightHittable hittable) => ignoreHittables.Contains(hittable);

        public void ExistsWhen(Func<bool> predicate)
        {
            existencePredicates.Add(predicate);
        }

        public bool CheckExists()
        {
            Func<bool> predicates = () => existencePredicates.Count == 0
                                       || existencePredicates.TrueForAll(predicate => predicate());

            if (IsActive && predicates()) return true;

            Destroy();
            return false;
        }
    }
}