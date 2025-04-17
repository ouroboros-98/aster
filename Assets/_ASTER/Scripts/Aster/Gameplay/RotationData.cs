using Aster.Utils;
using UnityEngine;

namespace Aster.Core
{
    public class RotationData
    {
        public readonly Angle From;
        public readonly Angle To;
        public readonly float AbsDelta;
        public readonly int   Direction;

        private float _progress;
        private float NormalizedProgress => Mathf.Clamp01(_progress / AbsDelta);
        public  bool  IsFinished         => NormalizedProgress >= 1f;

        public RotationData(Angle from, Angle to)
        {
            From = from;
            To   = to;

            float shortestDistance = Angle.ShortestDistance(from, to);

            AbsDelta  = Mathf.Abs(shortestDistance);
            Direction = (int)Mathf.Sign(shortestDistance);

            _progress = 0f;
        }

        public Angle GetAngleAddProgress(float progress)
        {
            _progress += progress;
            return Angle.Lerp(From, To, NormalizedProgress);
        }

        public static implicit operator RotationData((Angle from, Angle to) tuple)
        {
            return new RotationData(tuple.from, tuple.to);
        }
    }
}