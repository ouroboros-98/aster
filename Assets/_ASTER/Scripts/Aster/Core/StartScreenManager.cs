using System;
using Aster.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace _ASTER.Prefabs.Core
{
    public class StartSceneManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        [SerializeField] private float secondTransitionDelay = 2f;
        [SerializeField] private float firstTransitionDuration = 6f;
        [SerializeField] private float secondTransitionDuration = 2f;

        private Vector3 cameraStartPos = new Vector3(0.03f, 3.62f, -0.18f);
        private Vector3 cameraStartRot = new Vector3(90, 0, 0);
        private Vector3 cameraFinalPos1 = new Vector3(0, 9.35f, 0);
        private Vector3 cameraFinalPos2 = new Vector3(0, 12.6f, -15.8f);
        private Vector3 cameraFinalRot = new Vector3(45.13f, 0, 0);

        private bool hasStarted = false;

        private void Start()
        {
            mainCamera.transform.position = cameraStartPos;
            mainCamera.transform.rotation = Quaternion.Euler(cameraStartRot);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {

                // Step 1: Move camera to first position
                mainCamera.transform.DOMove(cameraFinalPos1, firstTransitionDuration).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        // Wait 4 seconds before starting Step 2
                        // Step 2: Wait, then move camera again and rotate
                        DOVirtual.DelayedCall(secondTransitionDelay, () =>
                        {
                            // Step 2: Move to cameraFinalPos2 AND rotate
                            Sequence cameraSequence = DOTween.Sequence();
                            cameraSequence.Append(mainCamera.transform.DOMove(cameraFinalPos2, secondTransitionDuration).SetEase(Ease.InOutSine));
                            cameraSequence.Join(mainCamera.transform.DORotate(cameraFinalRot, secondTransitionDuration).SetEase(Ease.InOutSine));
                            cameraSequence.OnComplete(() =>
                            {
                                AsterEvents.Instance.OnGameStartComplete?.Invoke();
                            });
                        });
                    });                        
            }
        }
    }
}