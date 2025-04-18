using System;
using ImprovedTimers;
using UnityEngine;

namespace Aster.Utils
{
    [System.Serializable]
    public class SerializableTimer
    {
        [SerializeField] private float _duration;

        private CountdownTimer _countdownTimer;

        public float CurrentTime => _countdownTimer?.CurrentTime ?? 0;
        public bool  IsRunning   => _countdownTimer?.IsRunning   ?? false;
        public bool  IsFinished  => _countdownTimer?.IsFinished  ?? false;
        public float Progress    => _countdownTimer?.Progress    ?? 0;

        public event Action OnTimerStart;
        public event Action OnTimerStop;

        public SerializableTimer(float duration)
        {
            _duration       = duration;
            _countdownTimer = new CountdownTimer(duration);

            OnTimerStart = delegate { };
            OnTimerStop  = delegate { };

            _countdownTimer.OnTimerStart += OnTimerStartCB;
            _countdownTimer.OnTimerStop  += OnTimerStopCB;
        }

        private void OnTimerStartCB() => OnTimerStart?.Invoke();
        private void OnTimerStopCB()  => OnTimerStop?.Invoke();

        public void Start()
        {
            Reset();
            _countdownTimer.Start();
        }

        public  void Pause()  => _countdownTimer.Pause();
        public  void Resume() => _countdownTimer.Resume();
        public  void Stop()   => _countdownTimer.Stop();
        private void Reset()  => _countdownTimer.Reset(_duration);
    }
}