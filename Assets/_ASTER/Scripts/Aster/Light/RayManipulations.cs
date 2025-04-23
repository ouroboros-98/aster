using System;
using UnityEngine;

namespace Aster.Light
{
    public abstract partial class RayManipulation
    {
        protected static class Manipulate
        {
            private static readonly Action<LightRay, UnityEngine.Color> SET_COLOR = (ray, value) => ray.Color = value;
            private static readonly Action<LightRay, Vector3> SET_ORIGIN = (ray, value) => ray.Origin = value;
            private static readonly Action<LightRay, Vector3> SET_DIRECTION = (ray, value) => ray.Direction = value;
            private static readonly Action<LightRay, float> SET_INTENSITY = (ray, value) => ray.Intensity = value;
            private static readonly Action<LightRay, float> SET_WIDTH = (ray, value) => ray.Width = value;

            public static RayManipulation Origin(Func<LightHit, LightRay, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_ORIGIN);

            public static RayManipulation Origin(Func<LightHit, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_ORIGIN);

            public static RayManipulation Origin(Func<LightRay, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_ORIGIN);


            public static RayManipulation Direction(Func<LightHit, LightRay, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_DIRECTION);

            public static RayManipulation Direction(Func<LightHit, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_DIRECTION);

            public static RayManipulation Direction(Func<LightRay, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_DIRECTION);


            public static RayManipulation Intensity(Func<LightHit, LightRay, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_INTENSITY);

            public static RayManipulation Intensity(Func<LightHit, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_INTENSITY);

            public static RayManipulation Intensity(Func<LightRay, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_INTENSITY);


            public static RayManipulation Width(Func<LightHit, LightRay, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_WIDTH);

            public static RayManipulation Width(Func<LightHit, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_WIDTH);

            public static RayManipulation Width(Func<LightRay, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_WIDTH);


            public static RayManipulation Color(Func<LightHit, LightRay, UnityEngine.Color> calculate) =>
                new PropertyManipulation<UnityEngine.Color>(calculate, SET_COLOR);

            public static RayManipulation Color(Func<LightHit, UnityEngine.Color> calculate) =>
                new PropertyManipulation<UnityEngine.Color>(calculate, SET_COLOR);

            public static RayManipulation Color(Func<LightRay, UnityEngine.Color> calculate) =>
                new PropertyManipulation<UnityEngine.Color>(calculate, SET_COLOR);

            public class PropertyManipulation<T> : RayManipulation
            {
                private Action<LightRay, T> SetCallback;

                private Func<LightHit, LightRay, T> CalculateCallback;

                public PropertyManipulation(Func<LightHit, LightRay, T> calculateCallback,
                                            Action<LightRay, T>         setCallback)
                {
                    CalculateCallback = calculateCallback;
                    SetCallback       = setCallback;
                }

                public PropertyManipulation(Func<LightHit, T> calculate, Action<LightRay, T> setCallback)
                    : this((hit, ray) => calculate(hit), setCallback)
                {
                }

                public PropertyManipulation(Func<LightRay, T> calculate, Action<LightRay, T> setCallback)
                    : this((hit, ray) => calculate(ray), setCallback)
                {
                }

                public override void Apply(LightHit hit, LightRay rayIn, LightRay rayOut) =>
                    SetCallback(rayOut, CalculateCallback(hit, rayIn));
            }
        }
    }
}