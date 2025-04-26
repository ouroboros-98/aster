using System;
using Aster.Core;
using Aster.Core.Entity;
using Aster.Utils;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Light
{
    public class MainLightSource : AsterSingleton<MainLightSource>
    {
        [SerializeField, BoxedProperty, Label("HP")] protected EntityHP  hp;
        [SerializeField]                             private   Transform collectionPoint;
        [SerializeField]                             private   float     radius;

        public Transform CollectionPoint => collectionPoint;

        private float firstRadius, secondRadius, thirdRadius;

        private void Awake()
        {
            firstRadius  = radius      / 3f;
            secondRadius = radius * 2f / 3f;
            thirdRadius  = radius;
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

        private void OnEnable()
        {
            AsterEvents.Instance.OnAttackLightSource += GotHit;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnAttackLightSource -= GotHit;
        }

        private void GotHit(int damage)
        {
            hp.ChangeBy(-damage);
            Debug.Log("Current Light Source HP: " + (int)hp); // uses the implicit int cast

            if (hp <= 0)
            {
                AsterEvents.Instance.OnLightSourceDestroyed?.Invoke();
            }
        }
    }
}