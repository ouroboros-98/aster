using System;
using ImprovedTimers;
using TNRD;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    public interface IWaveTimingHandler
    {
        void OnPrestart(WaveExecutionContext context);
        bool CanStart();
    }

    [Serializable]
    public class DelayedTimingHandler : IWaveTimingHandler
    {
        [SerializeField] private float delay = 0f;

        [SerializeReference, SerializeReferenceDropdown] private IWaveTimingHandler innerHandler;

        private bool _finishDelay = false;

        private CountdownTimer _timer;

        public void OnPrestart(WaveExecutionContext context)
        {
            innerHandler?.OnPrestart(context);
        }

        public bool CanStart()
        {
            if (_finishDelay) return true;

            if (innerHandler != null && !innerHandler.CanStart())
            {
                return false;
            }
            else if (_timer == null)
            {
                _timer             =  new CountdownTimer(delay);
                _timer.OnTimerStop += () => _finishDelay = true;
                _timer.Start();
            }

            return false;
        }
    }

    public class AfterPreviousTimingHandler : IWaveTimingHandler
    {
        private WaveExecutionContext context;

        public void OnPrestart(WaveExecutionContext context)
        {
            this.context = context;
        }

        public bool CanStart()
        {
            return context == null || context.Previous == null || context.Previous.Status == WaveStatus.Done;
        }
    }

    public class WithPreviousTimingHandler : IWaveTimingHandler
    {
        private IWaveElement _lastWaveElement;

        public void OnPrestart(WaveExecutionContext context)
        {
            _lastWaveElement = context.Previous;
        }

        public bool CanStart()
        {
            return _lastWaveElement == null || _lastWaveElement.Status == WaveStatus.InProgress;
        }
    }
}