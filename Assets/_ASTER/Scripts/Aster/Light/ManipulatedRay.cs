using System;
using System.Collections.Generic;
using Aster.Utils;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Aster.Light
{
    public abstract class ManipulatedRay : ILightRay
    {
        private readonly ILightRay       _innerRay;
        private          RayManipulation _manipulation;


        public ManipulatedRay(ILightRay innerRay, RayManipulation manipulation) : base()
        {
            _innerRay     = innerRay;
            _manipulation = manipulation;
        }

        public ManipulatedRay(LightHit hit, RayManipulation manipulation) : this(hit.Ray, manipulation)
        {
            UpdateTransformation(hit);
        }

        public abstract void UpdateTransformation(LightHit hit);

        public IReadOnlyList<Func<bool>> ExistencePredicates => _innerRay.ExistencePredicates;
        public bool                      IsActive            => _innerRay.IsActive;

        public Vector3 Origin
        {
            get => _innerRay.Origin;
            set => _innerRay.Origin = value;
        }

        public Vector3 EndPoint
        {
            get => _innerRay.EndPoint;
            set => _innerRay.EndPoint = value;
        }

        public float Intensity
        {
            get => _innerRay.Intensity;
            set => _innerRay.Intensity = value;
        }

        public float Width
        {
            get => _innerRay.Width;
            set => _innerRay.Width = value;
        }

        public Color Color
        {
            get => _innerRay.Color;
            set => _innerRay.Color = value;
        }

        public Vector3Normalized Direction
        {
            get => _innerRay.Direction;
            set => _innerRay.Direction = value;
        }

        public event Action<Vector3> OriginChange
        {
            add => _innerRay.OriginChange += value;
            remove => _innerRay.OriginChange -= value;
        }

        public event Action<Vector3> EndPointChange
        {
            add => _innerRay.EndPointChange += value;
            remove => _innerRay.EndPointChange -= value;
        }

        public event Action<float> IntensityChange
        {
            add => _innerRay.IntensityChange += value;
            remove => _innerRay.IntensityChange -= value;
        }

        public event Action<float> WidthChange
        {
            add => _innerRay.WidthChange += value;
            remove => _innerRay.WidthChange -= value;
        }

        public event Action<Color> ColorChange
        {
            add => _innerRay.ColorChange += value;
            remove => _innerRay.ColorChange -= value;
        }

        public event Action<Vector3Normalized> DirectionChange
        {
            add => _innerRay.DirectionChange += value;
            remove => _innerRay.DirectionChange -= value;
        }

        public event Action OnDestroy
        {
            add => _innerRay.OnDestroy += value;
            remove => _innerRay.OnDestroy -= value;
        }

        public void Activate()    => _innerRay.Activate();
        public void Destroy()     => _innerRay.Destroy();
        public void ForceUpdate() => _innerRay.ForceUpdate();

        public void Set(ILightRay ray) => _innerRay.Set(ray);

        public ILightRay Clone(bool activate = true) => _innerRay.Clone(activate);

        public ILightRay Continue()
        {
            if (!IsActive) return null;

            ILightRay continuationRay = _innerRay.Continue();
            OnDestroy += continuationRay.Destroy;

            return continuationRay;
        }

        public void IgnoreHittable(BaseLightHittable hittable) => _innerRay.IgnoreHittable(hittable);

        public bool CheckIgnoreHittable(BaseLightHittable hittable) => _innerRay.CheckIgnoreHittable(hittable);

        public void ExistsWhen(Func<bool> predicate) => _innerRay.ExistsWhen(predicate);

        public bool CheckExists() => _innerRay.CheckExists();
    }

    public class ManipulatedRay<TManipulation> : ManipulatedRay, ILightRay where TManipulation : RayManipulation
    {
        private TManipulation _manipulation;

        protected ManipulatedRay(ILightRay source, TManipulation manipulation) : base(source, manipulation)
        {
            _manipulation = manipulation;
        }

        public ManipulatedRay(LightHit source, TManipulation manipulation) : this(source.Ray, manipulation)
        {
            _manipulation = manipulation;
        }

        public override void UpdateTransformation(LightHit hit)
        {
            if (!this.IsActive) return;

            _manipulation.Apply(hit, this);
        }
    }

    public class ContinuedRay<TTransformation> : ManipulatedRay<TTransformation>
        where TTransformation : RayManipulation
    {
        public ContinuedRay(LightHit source, TTransformation manipulation) : base(source.Ray.Continue(), manipulation)
        {
        }
    }
}