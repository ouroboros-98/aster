using System;
using UnityEngine;

namespace Aster.Light
{
    public abstract partial class RayManipulation
    {
        protected static class Manipulate
        {
            private static readonly Action<ILightRay, UnityEngine.Color> SET_COLOR = (ray, value) => ray.Color = value;
            private static readonly Action<ILightRay, Vector3> SET_ORIGIN = (ray, value) => ray.Origin = value;
            private static readonly Action<ILightRay, Vector3> SET_DIRECTION = (ray, value) => ray.Direction = value;
            private static readonly Action<ILightRay, float> SET_INTENSITY = (ray, value) => ray.Intensity = value;
            private static readonly Action<ILightRay, float> SET_WIDTH = (ray, value) => ray.Width = value;

            public static RayManipulation Origin(Func<LightHit, ILightRay, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_ORIGIN);

            public static RayManipulation Origin(Func<LightHit, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_ORIGIN);

            public static RayManipulation Origin(Func<ILightRay, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_ORIGIN);


            public static RayManipulation Direction(Func<LightHit, ILightRay, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_DIRECTION);

            public static RayManipulation Direction(Func<LightHit, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_DIRECTION);

            public static RayManipulation Direction(Func<ILightRay, Vector3> calculate) =>
                new PropertyManipulation<Vector3>(calculate, SET_DIRECTION);


            public static RayManipulation Intensity(Func<LightHit, ILightRay, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_INTENSITY);

            public static RayManipulation Intensity(Func<LightHit, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_INTENSITY);

            public static RayManipulation Intensity(Func<ILightRay, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_INTENSITY);


            public static RayManipulation Width(Func<LightHit, ILightRay, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_WIDTH);

            public static RayManipulation Width(Func<LightHit, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_WIDTH);

            public static RayManipulation Width(Func<ILightRay, float> calculate) =>
                new PropertyManipulation<float>(calculate, SET_WIDTH);


            public static RayManipulation Color(Func<LightHit, ILightRay, UnityEngine.Color> calculate) =>
                new PropertyManipulation<UnityEngine.Color>(calculate, SET_COLOR);

            public static RayManipulation Color(Func<LightHit, UnityEngine.Color> calculate) =>
                new PropertyManipulation<UnityEngine.Color>(calculate, SET_COLOR);

            public static RayManipulation Color(Func<ILightRay, UnityEngine.Color> calculate) =>
                new PropertyManipulation<UnityEngine.Color>(calculate, SET_COLOR);

            public class PropertyManipulation<T> : RayManipulation
            {
                private Action<ILightRay, T> SetCallback;

                private Func<LightHit, ILightRay, T> CalculateCallback;

                public PropertyManipulation(Func<LightHit, ILightRay, T> calculateCallback,
                                            Action<ILightRay, T>         setCallback)
                {
                    CalculateCallback = calculateCallback;
                    SetCallback       = setCallback;
                }

                public PropertyManipulation(Func<LightHit, T> calculate, Action<ILightRay, T> setCallback)
                    : this((hit, ray) => calculate(hit), setCallback)
                {
                }

                public PropertyManipulation(Func<ILightRay, T> calculate, Action<ILightRay, T> setCallback)
                    : this((hit, ray) => calculate(ray), setCallback)
                {
                }

                public override void Apply(LightHit hit, ILightRay rayIn, ILightRay rayOut) =>
                    SetCallback(rayOut, CalculateCallback(hit, rayIn));
            }
        }
    }
}