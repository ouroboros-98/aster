using System;
using Aster.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aster.Utils
{
    public class SetYToLightY : AsterMono
    {
        [SerializeField]
        private Transform targetTransform;

        private void Update()
        {
            ValidateY();
        }

        [Button("Update")]
        private void ValidateY()
        {
            if (!Mathf.Approximately(targetTransform.position.y, Config.LightRayYPosition))
            {
                targetTransform.position = targetTransform.position.With(y: Config.LightRayYPosition);
            }
        }
    }
}