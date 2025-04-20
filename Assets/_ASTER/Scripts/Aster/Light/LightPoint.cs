using System;
using UnityEngine;

namespace Aster.Light
{
    public class LightPoint : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private Transform lightSource;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float arrivalThreshold = 0.1f;

        public event Action OnArrivedAtLightSource;

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
            OnArrivedAtLightSource?.Invoke();
            Destroy(gameObject); 
        }
    }
}