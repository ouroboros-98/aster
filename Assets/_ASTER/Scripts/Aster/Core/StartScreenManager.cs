using System;
using Aster.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _ASTER.Prefabs.Core
{
    public class StartSceneManager : AsterMono
    {
        [SerializeField] private Camera mainCamera;

        [SerializeField] private float secondTransitionDelay = 0.5f;
        [SerializeField] private float firstTransitionDuration = 6f;
        [SerializeField] private float secondTransitionDuration = 2f;
        [SerializeField] private Image[] tutorialImages;
        [SerializeField] private bool setShouldBegin = true;
        [SerializeField] private InputActionProperty switchSceneAction;
        [SerializeField] private InputActionProperty quitSceneAction;
        private Vector3 cameraStartPos = new Vector3(0.03f, 3.869f, -0.785f);
        private Vector3 cameraStartRot = new Vector3(90, 0, 0);
        private Vector3 cameraFinalPos1 = new Vector3(0, 9.35f, -0.785f);
        private Vector3 cameraFinalPos2 = new Vector3(0, 12.6f, -15.8f);
        private Vector3 cameraFinalRot = new Vector3(45.13f, 0, 0);
        private int tutorialIndex = -1;
        private bool hasStarted = false;

        private void Start()
        {
            if (!Config.EnableTitleScreen|| !setShouldBegin)
            {
                StartGameCompleted();
                return;
            }
            MoveCameraToTitleScreenState();
        }

        private void OnEnable()
        {
            switchSceneAction.action.performed += Updatee;
            quitSceneAction.action.performed += QuitGame;
            quitSceneAction.action.Enable();
            switchSceneAction.action.Enable();
        }

        private void QuitGame(InputAction.CallbackContext ctx)
        {
        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
        }

        private void MoveCameraToTitleScreenState()
        {
            mainCamera.transform.position = cameraStartPos;
            mainCamera.transform.rotation = Quaternion.Euler(cameraStartRot);
        }

        private void Updatee(InputAction.CallbackContext ctx)
        {
            switch (hasStarted)
            {
                case false:
                    quitSceneAction.action.Disable();
                    mainCamera.transform.DOMove(cameraFinalPos1, firstTransitionDuration).SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {
                            // Wait 4 seconds before starting Step 2
                            // Step 2: Wait, then move camera again and rotate
                            //DOVirtual.DelayedCall(secondTransitionDelay, () =>
                            //{
                            // Step 2: Move to cameraFinalPos2 AND rotate
                            Sequence cameraSequence = DOTween.Sequence();
                            cameraSequence.Append(mainCamera.transform.DOMove(cameraFinalPos2, secondTransitionDuration).SetEase(Ease.InOutSine));
                            cameraSequence.Join(mainCamera.transform.DORotate(cameraFinalRot, secondTransitionDuration).SetEase(Ease.InOutSine));
                            cameraSequence.OnComplete(() =>
                            {
                                // AsterEvents.Instance.OnGameStartComplete?.Invoke();
                                hasStarted= true;
                                tutorialIndex++;
                                tutorialImages[tutorialIndex].gameObject.SetActive(true);
                            });
                            //});
                        });
                    break;
                case true:
                {
                    tutorialIndex++;
                    if (tutorialIndex >= tutorialImages.Length)
                    {
                        tutorialImages[tutorialIndex - 1].gameObject.SetActive(false);
                        tutorialIndex = -1;
                        switchSceneAction.action.Disable();
                        StartGameCompleted();
                    }
                    else
                    {
                        if(tutorialIndex-1 >= 0)
                            tutorialImages[tutorialIndex - 1].gameObject.SetActive(false);
                        tutorialImages[tutorialIndex].gameObject.SetActive(true);
                    }

                    break;
                }
            }
        }

        private void StartGameCompleted()
        {
            hasStarted = true;
            GameEvents.OnGameStartComplete?.Invoke();
        }

        public void setupShouldBegin(bool shouldBegin)
        {
            setShouldBegin = shouldBegin;
        }
    }
}