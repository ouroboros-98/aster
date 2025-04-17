using System;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Core.Entity
{
    [System.Serializable]
    public class EntityHP
    {
        public readonly struct HPChangeContext
        {
            public readonly int CurrentValue;
            public readonly int PreviousValue;
            public readonly int Delta;

            public HPChangeContext(int currentValue, int previousValue, int delta)
            {
                CurrentValue  = currentValue;
                PreviousValue = previousValue;
                Delta         = delta;
            }
        }

        [SerializeField, OnValueChanged("Set")] private int _current;

        [SerializeField] private int _max;

        public event Action<HPChangeContext> OnHPChange;

        public int MaxHP => _max;

        public EntityHP(int max, int? current = null)
        {
            max  = (max < 1) ? 1 : max;
            _max = max;

            if (current != null) SetHP(current.Value, false);
            else _current = max;

            OnHPChange = delegate { };
        }

        private int SetHP(int targetValue, bool triggerEvent = true)
        {
            int previousValue = _current;

            int value = targetValue;

            value = (value < 0) ? 0 : value;
            value = (value > _max) ? _max : value;

            _current = value;

            int delta = _current - previousValue;

            if (triggerEvent)
            {
                HPChangeContext context = new(_current, previousValue, delta);
                OnHPChange?.Invoke(context);
            }

            return value;
        }

        public void Set(int      targetValue) => SetHP(targetValue);
        public void ChangeBy(int delta)       => SetHP(_current + delta);

        public static implicit operator int(EntityHP hp) => hp._current;
    }
}