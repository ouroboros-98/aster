using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Aster.Core.Entity
{
    [System.Serializable]
    public class EntityHP
    {
        public readonly struct HPChangeContext
        {
            public readonly float CurrentValue;
            public readonly float PreviousValue;
            public readonly float Delta;
            public readonly float MaxValue;

            public HPChangeContext(float currentValue, float previousValue, float delta, float maxValue)
            {
                CurrentValue  = currentValue;
                PreviousValue = previousValue;
                Delta         = delta;
                MaxValue      = maxValue;
            }
        }

        [SerializeField, OnValueChanged("Set")] private float _current;

        [SerializeField] private float _max;

        public event Action<HPChangeContext>                 OnHPChange;
        [SerializeField] private UnityEvent<HPChangeContext> _onHPChange;

        public float MaxHP => _max;

        public EntityHP(float max, float? current = null)
        {
            max  = (max < 1) ? 1 : max;
            _max = max;

            if (current != null) SetHP(current.Value, false);
            else _current = max;

            OnHPChange = delegate { };
        }

        private float SetHP(float targetValue, bool triggerEvent = true)
        {
            float previousValue = _current;

            float value = targetValue;

            value = (value < 0) ? 0 : value;
            value = (value > _max) ? _max : value;

            _current = value;

            float delta = _current - previousValue;

            if (triggerEvent)
            {
                HPChangeContext context = new(_current, previousValue, delta, _max);
                OnHPChange?.Invoke(context);
                _onHPChange?.Invoke(context);
            }

            return value;
        }

        public void Set(float      targetValue) => SetHP(targetValue);
        public void ChangeBy(float delta)       => SetHP(_current + delta);

        public static implicit operator float(EntityHP hp) => hp._current;
    }
}