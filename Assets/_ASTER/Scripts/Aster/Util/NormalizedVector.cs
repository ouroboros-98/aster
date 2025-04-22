using System;
using UnityEngine;

namespace Aster.Utils
{
    public struct Vector2Normalized : IEquatable<Vector2Normalized>
    {
        private Vector2 _value;

        public static implicit operator Vector2Normalized(Vector2 value) => new() { _value = value.normalized };
        public static implicit operator Vector2(Vector2Normalized value) => value._value;

        public static Vector2 operator *(Vector2Normalized value, float multiplier) => value._value * multiplier;
        public static Vector2 operator *(float multiplier, Vector2Normalized value) => value._value * multiplier;

        public static Vector2 operator /(Vector2Normalized value, float divisor)
        {
            if (Mathf.Approximately(divisor, 0f))
                throw new DivideByZeroException("Cannot divide by zero.");
            return value._value / divisor;
        }

        public bool Equals(Vector2Normalized other) => _value.Equals(other._value);

        public override bool Equals(object obj) => obj is Vector2Normalized other && Equals(other);
        public override int  GetHashCode()      => _value.GetHashCode();

        public static bool operator ==(Vector2Normalized left, Vector2Normalized right) => left.Equals(right);
        public static bool operator !=(Vector2Normalized left, Vector2Normalized right) => !left.Equals(right);
    }

    public struct Vector3Normalized : IEquatable<Vector3Normalized>
    {
        private Vector3 _value;

        public static implicit operator Vector3Normalized(Vector3 value) => new() { _value = value.normalized };
        public static implicit operator Vector3(Vector3Normalized value) => value._value;

        public static Vector3 operator *(Vector3Normalized value, float multiplier) => value._value * multiplier;
        public static Vector3 operator *(float multiplier, Vector3Normalized value) => value._value * multiplier;

        public static Vector3 operator /(Vector3Normalized value, float divisor)
        {
            if (Mathf.Approximately(divisor, 0f))
                throw new DivideByZeroException("Cannot divide by zero.");
            return value._value / divisor;
        }

        public bool Equals(Vector3Normalized other) => _value.Equals(other._value);

        public override bool Equals(object obj) => obj is Vector3Normalized other && Equals(other);
        public override int  GetHashCode()      => _value.GetHashCode();

        public static bool operator ==(Vector3Normalized left, Vector3Normalized right) => left.Equals(right);
        public static bool operator !=(Vector3Normalized left, Vector3Normalized right) => !left.Equals(right);
    }
}