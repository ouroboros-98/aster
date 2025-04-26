using System;
using Aster.Core;
using Aster.Utils.Pool;
using DG.Tweening;
using UnityEngine;

namespace Aster.Light
{
    public class LightPoint : AsterMono, IPoolable
    {
        [SerializeField] private float     speed = 5f;
        [SerializeField] private Transform lightSource;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float     arrivalThreshold = 0.1f;

        private bool  _isMovingToLightSource = false;
        private Tween _movementTween;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                SoundManager.Instance.Play("EnergyTaken");
                StartMovingToLightSource();
            }
        }

        private void StartMovingToLightSource()
        {
            // _isMovingToLightSource = true;
            rb.isKinematic = true; // So physics doesn't interfere

            Vector3 targetPosition = lightSource.position;

            var tweenX = transform.DOMoveX(targetPosition.x, 1 / speed);
            var tweenY = transform.DOMoveY(targetPosition.x, 1 / speed).SetEase(Ease.OutBack);
            var tweenZ = transform.DOMoveZ(targetPosition.x, 1 / speed);

            _movementTween = DOTween.Sequence()
                                    .Join(tweenX)
                                    .Join(tweenY)
                                    .Join(tweenZ)
                                    .OnComplete(() =>
                                                {
                                                    _isMovingToLightSource = false;
                                                    rb.isKinematic         = false;
                                                });
        }

        private void Update()
        {
            // if (_isMovingToLightSource)
            // {
            //     Vector3 direction = (lightSource.position - transform.position).normalized;
            //     transform.position += direction * (speed * Time.deltaTime);
            //
            //     if (Vector3.Distance(transform.position, lightSource.position) < arrivalThreshold)
            //     {
            //         ArriveAtLightSource();
            //     }
            // }
        }

        private void FixedUpdate()
        {
            if (_movementTween      != null) return;
            if (rb.linearVelocity.y != 0) return;

            rb.linearVelocity = Vector3.zero;
        }

        private void ArriveAtLightSource()
        {
            Debug.Log("ArriveAtLightSource");

            AsterEvents.Instance.OnLightPointAdded?.Invoke(1);

            EnergyPool.Instance.Return(this);
        }

        public void Reset()
        {
            _movementTween?.Kill();
            _movementTween = null;

            lightSource            = MainLightSource.Instance.CollectionPoint;
            _isMovingToLightSource = false;
            rb.isKinematic         = false;
        }
    }
}