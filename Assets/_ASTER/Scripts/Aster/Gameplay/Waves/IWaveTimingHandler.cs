using System;
using Aster.Core;
using ImprovedTimers;
using TNRD;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    public interface IWaveTimingHandler
    {
        void OnPrestart(WaveExecutionContext context);
        bool CanStart();

        void Reset();
    }

    [Serializable]
    public abstract class DelayableTimingHandler : IWaveTimingHandler
    {
        [SerializeField, Range(0f, 5f)] private float delay = 0f;

        [SerializeField] private IWaveTimingHandler innerHandler;

        private bool _finishDelay = false;

        private CountdownTimer _timer;

        protected DelayableTimingHandler(IWaveTimingHandler innerHandler)
        {
            this.innerHandler = innerHandler;
        }

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

        public void Reset()
        {
            _finishDelay = false;
            _timer       = null;
            innerHandler?.Reset();
        }
    }

    public class AfterPrevious : DelayableTimingHandler
    {
        public AfterPrevious() : base(new AfterPreviousTimingHandlerBase())
        {
        }
    }

    public class WithPrevious : DelayableTimingHandler
    {
        public WithPrevious() : base(new WithPreviousTimingHandlerBase())
        {
        }
    }

    class AfterPreviousTimingHandlerBase : IWaveTimingHandler
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

        public void Reset()
        {
            context = null;
        }
    }

    class WithPreviousTimingHandlerBase : IWaveTimingHandler
    {
        private WaveExecutionContext context;

        private bool canStart = false;

        public void OnPrestart(WaveExecutionContext context)
        {
            this.context                     =  context;
            AsterEvents.Instance.OnWaveStart += OnWaveStart;
        }

        public void OnWaveStart(int obj)
        {
            if (obj == context.WaveIndex - 1)
            {
                canStart = true;

                AsterEvents.Instance.OnWaveStart -= OnWaveStart;
            }
        }

        public bool CanStart()
        {
            return canStart;
        }

        public void Reset()
        {
            context  = null;
            canStart = false;
        }
    }
}