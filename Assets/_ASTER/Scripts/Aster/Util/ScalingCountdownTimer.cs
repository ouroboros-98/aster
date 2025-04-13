using System;
using ImprovedTimers;
using UnityEngine;

namespace Aster.Utils
{
    public class ScalingCountdownTimer : Timer
    {
        public Func<float> ScaleGetter;
        public float       Scale => ScaleGetter != null ? ScaleGetter() : 1;

        public ScalingCountdownTimer(float value) : base(value)
        {
        }


        public override void Tick()
        {
            if (IsRunning && CurrentTime > 0)
            {
                CurrentTime -= Time.deltaTime * Scale;
            }

            if (IsRunning && CurrentTime <= 0)
            {
                Stop();
            }
        }

        public override bool IsFinished => CurrentTime <= 0;
    }
}