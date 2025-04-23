using System;
using Aster.Core;
using Aster.Utils.Pool;
using UnityEngine;

namespace Aster.Light
{
    public class LightPoint : AsterMono, IPoolable
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private Transform lightSource;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float arrivalThreshold = 0.1f;
        
        private bool _isMovingToLightSource = false;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartMovingToLightSource();
            }
        }

        private void StartMovingToLightSource()
        {
            _isMovingToLightSource = true;
            rb.isKinematic = true; // So physics doesn't interfere
        }

        private void Update()
        {
            if (_isMovingToLightSource)
            {
                Vector3 direction = (lightSource.position - transform.position).normalized;
                transform.position += direction * (speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, lightSource.position) < arrivalThreshold)
                {
                    ArriveAtLightSource();
                }
            }
        }

        private void ArriveAtLightSource()
        {
            Debug.Log("ArriveAtLightSource");
            AsterEvents.Instance.OnLightPointAdded?.Invoke(this);
            Destroy(gameObject); 
        }

        public void Reset()
        {
            _isMovingToLightSource = false;
            rb.isKinematic = false;
        }
    }
}