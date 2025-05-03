using System;
using System.Collections.Generic;
using Aster.Core;
using Aster.Utils;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Light
{
    [System.Serializable]
    public class LightRay : ILightRay
    {
        public static readonly Color DEFAULT_COLOR = Color.white;

        public const float MAX_DISTANCE      = 5f;
        public const float DEFAULT_INTENSITY = 1f;
        public const float DEFAULT_WIDTH     = .2f;

        [OnValueChanged("Editor__OnOriginChanged")] [SerializeField] private Vector3 _origin = Vector3.zero;

        [OnValueChanged("Editor__OnEndPointChanged")] [SerializeField] private Vector3 _endPoint = Vector3.forward;

        [OnValueChanged("Editor__OnIntensityChanged")] [SerializeField] private float _intensity = DEFAULT_INTENSITY;

        [OnValueChanged("Editor__OnWidthChanged")] [SerializeField] private float _width = DEFAULT_WIDTH;

        [OnValueChanged("Editor__OnColorChanged")] [SerializeField] private Color _color = DEFAULT_COLOR;

        [OnValueChanged("Editor_OnDirectionChanged")] [SerializeField]
        private Vector3Normalized _direction = Vector3.forward;

        [OnValueChanged("Editor__OnMaxDistanceChanged")] [SerializeField] private float _maxDistance = MAX_DISTANCE;

        [SerializeField] private List<BaseLightHittable> ignoreHittables;

        private List<Func<bool>>                 existencePredicates;
        public  IReadOnlyList<Func<bool>>        ExistencePredicates => existencePredicates;
        public  IReadOnlyList<BaseLightHittable> HittablesToIgnore   => ignoreHittables;

        public bool IsActive { get; private set; }


#if UNITY_EDITOR
        private void Editor__OnOriginChanged()      => OriginChange?.Invoke(_origin);
        private void Editor__OnEndPointChanged()    => EndPointChange?.Invoke(_endPoint);
        private void Editor__OnIntensityChanged()   => IntensityChange?.Invoke(_intensity);
        private void Editor__OnWidthChanged()       => WidthChange?.Invoke(_width);
        private void Editor__OnColorChanged()       => ColorChange?.Invoke(_color);
        private void Editor__OnDirectionChanged()   => DirectionChange?.Invoke(_direction);
        private void Editor__OnMaxDistanceChanged() => DirectionChange?.Invoke(_direction);
#endif

        public virtual Vector3 Origin
        {
            get => _origin;
            set
            {
                if (_origin == value) return;
                _origin = value;
                OriginChange?.Invoke(_origin);
            }
        }

        public virtual Vector3 EndPoint
        {
            get => _endPoint;
            set
            {
                if (_endPoint == value) return;
                _endPoint = value;
                EndPointChange?.Invoke(_endPoint);
            }
        }

        public virtual float Intensity
        {
            get => _intensity;
            set
            {
                if (Mathf.Approximately(_intensity, value)) return;
                _intensity = value;
                IntensityChange?.Invoke(_intensity);
            }
        }

        public virtual float Width
        {
            get => _width;
            set
            {
                if (Mathf.Approximately(_width, value)) return;
                _width = value;
                WidthChange?.Invoke(_width);
            }
        }

        public virtual Color Color
        {
            get => _color;
            set
            {
                if (_color == value) return;
                _color = value;
                ColorChange?.Invoke(_color);
            }
        }

        public virtual float MaxDistance
        {
            get => _maxDistance;
            set
            {
                if (value < 0) value = 0;
                if (Mathf.Approximately(_maxDistance, value)) return;
                _maxDistance = value;
                MaxDistanceChange?.Invoke(_maxDistance);
                if (Vector3.Distance(Origin, EndPoint) > _maxDistance)
                {
                    Direction = (EndPoint - Origin).normalized;
                    EndPoint  = Origin + Direction * _maxDistance;
                }
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

        public event Action<Vector3>           OriginChange      = delegate { };
        public event Action<Vector3>           EndPointChange    = delegate { };
        public event Action<float>             IntensityChange   = delegate { };
        public event Action<float>             WidthChange       = delegate { };
        public event Action<Color>             ColorChange       = delegate { };
        public event Action<float>             MaxDistanceChange = delegate { };
        public event Action<Vector3Normalized> DirectionChange   = delegate { };

        public event Action OnDestroy = delegate { };

        private void OnDirectionChange(Vector3Normalized direction) => EndPoint = _origin + direction * MaxDistance;

        public LightRay(bool activate = true)
        {
            DirectionChange += OnDirectionChange;

            ignoreHittables     = new();
            existencePredicates = new();

            if (activate) Activate();
        }

        public LightRay(ILightRay source, bool activate = true) : this(false)
        {
            Set(source);

            if (activate) Activate();
        }

        public void Activate()
        {
            if (IsActive) return;

            IsActive = true;
            AsterEvents.Instance.OnRayActivated?.Invoke(this);
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

        public void Set(ILightRay ray)
        {
            this.Origin      = ray.Origin;
            this.MaxDistance = ray.MaxDistance;
            this.Direction   = ray.Direction;
            this.Intensity   = ray.Intensity;
            this.Width       = ray.Width;
            this.Color       = ray.Color;
            this.EndPoint    = ray.EndPoint;
        }

        public void IgnoreHittable(BaseLightHittable hittable)
        {
            if (ignoreHittables.Contains(hittable)) return;
            ignoreHittables.Add(hittable);
        }

        public virtual bool CheckIgnoreHittable(BaseLightHittable hittable) => ignoreHittables.Contains(hittable);

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

        public ILightRay Clone(bool activated = true) => new LightRay(this, activated);

        public ILightRay Continue()
        {
            if (!IsActive) return null;

            ILightRay ray = Clone();
            this.OnDestroy += ray.Destroy;
            return ray;
        }
    }
}