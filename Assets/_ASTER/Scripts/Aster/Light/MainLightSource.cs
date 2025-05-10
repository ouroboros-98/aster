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
        [SerializeField] private float damageReducedPerSecond;

        public Transform CollectionPoint => collectionPoint;

        private float firstRadius, secondRadius, thirdRadius;
        private bool _gameStarted = false;

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

        private void Update()
        {
            if (_gameStarted)
            {
                hp.ChangeBy(Time.deltaTime * -damageReducedPerSecond);
            }
            if (hp <= 0)
            {
                AsterEvents.Instance.OnLightSourceDestroyed?.Invoke();
            }
        }

        private void OnEnable()
        {
            AsterEvents.Instance.OnAttackLightSource += GotHit;
            AsterEvents.Instance.OnLightPointAdded += AddHp;
            AsterEvents.Instance.OnGameStartComplete += ChangeGameStartedFlag;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnAttackLightSource -= GotHit;
            AsterEvents.Instance.OnLightPointAdded -= AddHp;
            AsterEvents.Instance.OnGameStartComplete -= ChangeGameStartedFlag;
        }

        private void ChangeGameStartedFlag()
        {
            _gameStarted = true;
        }

        private void AddHp(int hpAdded)
        {
            if (hp + hpAdded > hp.MaxHP)
            {
                hp.ChangeBy(hp.MaxHP-hp);
            }
            else
            {
                hp.ChangeBy(hpAdded);
            }
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