using System;
using Aster.Core;
using Aster.Core.Entity;
using UnityEngine;

namespace Aster.Light
{
    public class MainLightSource : AsterMono
    {
        private EntityHP entityHp;
        [SerializeField] private float radius;
        private float firstRadius,secondRadius,thirdRadius;
        private void Awake()
        {
            entityHp = GetComponent<EntityHP>();
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