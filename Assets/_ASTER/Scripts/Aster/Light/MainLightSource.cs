using System;
using Aster.Core;
using Aster.Core.Entity;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Light
{
    public class MainLightSource : AsterMono
    {
        [SerializeField, BoxedProperty, Label("HP")] protected EntityHP       hp;
        [SerializeField] private float radius;
        private float firstRadius,secondRadius,thirdRadius;
        private void Awake()
        {
            firstRadius = radius / 3f;
            secondRadius = radius * 2f / 3f;
            thirdRadius = radius;
        }

        public int GetRadiusArea(GameObject target)
        {
            var distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance < firstRadius)
            {
                return 1;
            }
            if (distance < secondRadius)
            {
                return 2;
            }
            return distance < thirdRadius ? 3 : 0;
        }
        
    }
}