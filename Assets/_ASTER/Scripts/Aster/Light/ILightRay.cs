using System;
using System.Collections.Generic;
using Aster.Utils;
using UnityEngine;

namespace Aster.Light
{
    public interface ILightRay
    {
        IReadOnlyList<Func<bool>>       ExistencePredicates { get; }
        bool                            IsActive            { get; }
        Vector3                         Origin              { get; set; }
        Vector3                         EndPoint            { get; set; }
        float                           Intensity           { get; set; }
        float                           Width               { get; set; }
        Color                           Color               { get; set; }
        Vector3Normalized               Direction           { get; set; }
        event Action<Vector3>           OriginChange;
        event Action<Vector3>           EndPointChange;
        event Action<float>             IntensityChange;
        event Action<float>             WidthChange;
        event Action<Color>             ColorChange;
        event Action<Vector3Normalized> DirectionChange;
        event Action                    OnDestroy;
        void                            Activate();
        void                            Destroy();
        void                            ForceUpdate();
        void                            Set(ILightRay ray);

        ILightRay Clone(bool activate = true);
        ILightRay Continue();

        void IgnoreHittable(BaseLightHittable      hittable);
        bool CheckIgnoreHittable(BaseLightHittable hittable);
        void ExistsWhen(Func<bool>                 predicate);
        bool CheckExists();
    }
}