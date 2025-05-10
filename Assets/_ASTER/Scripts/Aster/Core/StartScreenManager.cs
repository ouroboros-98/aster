using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace _ASTER.Prefabs.Core
{
    public class StartSceneManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        [SerializeField] private float firstTransitionDuration = 6f;
        [SerializeField] private float secondTransitionDuration = 2f;
        [SerializeField] private string nextSceneName = "MainScene";

        private Vector3 cameraFinalPos1 = new Vector3(0, 12.6f, 0);
        private Vector3 cameraFinalPos2 = new Vector3(0, 12.6f, -15.8f);
        private Vector3 cameraFinalRot = new Vector3(45.13f, 0, 0);

        private bool hasStarted = false;

        private void Update()
        {
            if (!hasStarted && Input.GetKeyDown(KeyCode.X))
            {
                hasStarted = true;

                // Step 1: Move to cameraFinalPos1
                mainCamera.transform.DOMove(cameraFinalPos1, firstTransitionDuration).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        // Wait 4 seconds before starting Step 2
                        DOVirtual.DelayedCall(2f, () =>
                        {
                            // Step 2: Move to cameraFinalPos2 AND rotate
                            Sequence cameraSequence = DOTween.Sequence();
                            cameraSequence.Append(mainCamera.transform.DOMove(cameraFinalPos2, secondTransitionDuration).SetEase(Ease.InOutSine));
                            cameraSequence.Join(mainCamera.transform.DORotate(cameraFinalRot, secondTransitionDuration).SetEase(Ease.InOutSine));
                            cameraSequence.OnComplete(() =>
                            {
                                SceneManager.LoadScene(nextSceneName);
                            });
                        });
                    });
            }
        }
    }
}