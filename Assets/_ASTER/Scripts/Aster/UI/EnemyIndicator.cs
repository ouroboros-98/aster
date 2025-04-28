using System;
using Aster.Utils.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace Aster.Core.UI
{
    public class EnemyIndicator : AsterMono, IPoolable
    {
        private                  RectTransform indicatorRect;
        private                  Image         indicatorImage;
        [SerializeField] private float         offset = 50f;
        [SerializeField] private float         margin = -25f; // Extra margin to hide indicator sooner
        private                  Transform     _enemyTransform;
        private                  Camera        _mainCamera;

        public void Init(Transform enemyTransform)
        {
            _enemyTransform = enemyTransform;
        }

        public void Reset()
        {
        }

        public void OnEnable()
        {
            if (!indicatorImage)
            {
                indicatorImage = GetComponent<Image>();
            }

            if (!indicatorRect)
            {
                indicatorRect = GetComponent<RectTransform>();
            }
        }

        public void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (!_enemyTransform) IndicatorPool.Instance.Return(this);

            float   screenWidth  = Screen.width;
            float   screenHeight = Screen.height;
            Vector3 screenPos    = _mainCamera.WorldToScreenPoint(_enemyTransform.position);

            bool isOnScreen = screenPos.z > 0
                           && screenPos.x > margin
                           && screenPos.x < (Screen.width - margin)
                           && screenPos.y > margin
                           && screenPos.y < (Screen.height - margin);

            if (indicatorImage.enabled == isOnScreen)
            {
                indicatorImage.enabled = !isOnScreen;
            }

            if (!isOnScreen)
            {
                RefreshIndicator(screenPos, screenWidth, screenHeight);
            }
            else
            {
                IndicatorPool.Instance.Return(this);
            }
        }

        private void RefreshIndicator(Vector3 screenPos, float screenWidth, float screenHeight)
        {
            float halfWidth  = screenWidth  * 0.5f;
            float halfHeight = screenHeight * 0.5f;
            Vector2 fromCenterNormalized = new Vector2(
                                                       (screenPos.x - halfWidth)  / halfWidth,
                                                       (screenPos.y - halfHeight) / halfHeight
                                                      );

            float angle = Mathf.Atan2(fromCenterNormalized.y, fromCenterNormalized.x) * Mathf.Rad2Deg;
            Quaternion newRotation = angle switch
                                     {
                                         > -45f and <= 45f   => Quaternion.Euler(0f, 0f, 90f),
                                         > 45f and <= 135f   => Quaternion.Euler(0f, 0f, 180f),
                                         > -135f and <= -45f => Quaternion.Euler(0f, 0f, 0f),
                                         _                   => Quaternion.Euler(0f, 0f, 270f)
                                     };

            if (indicatorRect.rotation != newRotation)
            {
                indicatorRect.rotation = newRotation;
            }

            float   clampedX   = Mathf.Clamp(screenPos.x, -offset, screenWidth  + offset);
            float   clampedY   = Mathf.Clamp(screenPos.y, -offset, screenHeight + offset);
            Vector3 clampedPos = new Vector3(clampedX, clampedY, 0f);

            if (indicatorRect.position != clampedPos)
            {
                indicatorRect.position = clampedPos;
            }
        }
    }
}