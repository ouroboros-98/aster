using System;
using System.Linq;
using _ASTER.Scripts.Aster.Core;
using Aster.Core;
using Aster.Entity.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _ASTER.Prefabs.Core
{
    public class StartSceneManager : AsterMono
    {
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private float secondTransitionDelay = 0.5f;

        [SerializeField]
        private float firstTransitionDuration = 6f;

        [SerializeField]
        private float secondTransitionDuration = 2f;

        [SerializeField]
        private Image[] tutorialImages;

        [SerializeField]
        private bool setShouldBegin = true;

        [SerializeField]
        private FadeManager fadeManager;

        // [SerializeField] private InputActionProperty switchSceneAction;
        // [SerializeField] private InputActionProperty quitSceneAction;

        private PlayerInputHandler[] playerInputHandlers;

        private Vector3 cameraStartPos  = new Vector3(0.03f,  3.869f, -0.785f);
        private Vector3 cameraStartRot  = new Vector3(90,     0,      0);
        private Vector3 cameraFinalPos1 = new Vector3(0,      9.35f,  -0.785f);
        private Vector3 cameraFinalPos2 = new Vector3(0,      12.6f,  -15.8f);
        private Vector3 cameraFinalRot  = new Vector3(45.13f, 0,      0);

        private       int  tutorialIndex    = -1;
        private       bool hasStarted       = false;
        private       bool isMoving         = false;
        private       bool tutorialFinished = false;
        private       bool awaitGrab        = true;
        public static bool FirstTime        = true;

        private void Awake()
        {
            // fadeManager.SetFadeImageToBlack();


        }

        private void Start()
        {
            playerInputHandlers = FindObjectsOfType<PlayerController>()
                                 .Select(p => p.PlayerInputHandler)
                                 .ToArray();

            if (!FirstTime || !Config.EnableTitleScreen || !setShouldBegin)
            {
                StartGameCompleted();
                tutorialFinished = true;
                return;
            }

            MoveCameraToTitleScreenState();
            // fadeManager.FadeIn(() => { });
        }

        private void OnEnable()
        {
        }

        bool GrabButtonPressed()
        {
            foreach (PlayerInputHandler handler in playerInputHandlers)
            {
                if (handler.Grab.WasPressedThisFrame()) return true;
            }

            return false;
        }

        bool QuitButtonPreesed()
        {
            foreach (PlayerInputHandler handler in playerInputHandlers)
            {
                if (handler.Quit.WasPressedThisFrame()) return true;
            }

            return false;
        }

        private void HandleQuit()
        {
            if (!QuitButtonPreesed()) return;
            if (isMoving) return;
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

        void Update()
        {
            if (!FirstTime && hasStarted && tutorialFinished) return;
            HandleQuit();
            if (!awaitGrab) return;
            HandleGrab();
        }

        private void HandleGrab()
        {
            if (!GrabButtonPressed()) return;
            if (!hasStarted)
            {
                if (isMoving) return;

                isMoving = true;
                // quitSceneAction.action.Disable();
                mainCamera.transform.DOMove(cameraFinalPos1, firstTransitionDuration).SetEase(Ease.InOutSine)
                          .OnComplete(() =>
                                      {
                                          // Wait 4 seconds before starting Step 2
                                          // Step 2: Wait, then move camera again and rotate
                                          //DOVirtual.DelayedCall(secondTransitionDelay, () =>
                                          //{
                                          // Step 2: Move to cameraFinalPos2 AND rotate
                                          Sequence cameraSequence = DOTween.Sequence();
                                          cameraSequence.Append(mainCamera.transform
                                                                          .DOMove(cameraFinalPos2,
                                                                                    secondTransitionDuration)
                                                                          .SetEase(Ease.InOutSine));
                                          cameraSequence.Join(mainCamera.transform
                                                                        .DORotate(cameraFinalRot,
                                                                                  secondTransitionDuration)
                                                                        .SetEase(Ease.InOutSine));
                                          cameraSequence.OnComplete(() =>
                                                                    {
                                                                        // AsterEvents.Instance.OnGameStartComplete?.Invoke();
                                                                        hasStarted = true;
                                                                        tutorialIndex++;
                                                                        tutorialImages[tutorialIndex].gameObject
                                                                           .SetActive(true);
                                                                    });
                                          //});
                                      });
            }
            else if (!tutorialFinished)
            {
                tutorialIndex++;
                if (tutorialIndex >= tutorialImages.Length)
                {
                    tutorialImages[tutorialIndex - 1].gameObject.SetActive(false);
                    tutorialIndex    = -1;
                    awaitGrab        = false;
                    tutorialFinished = true;
                    StartGameCompleted();
                }
                else
                {
                    if (tutorialIndex - 1 >= 0)
                        tutorialImages[tutorialIndex - 1].gameObject.SetActive(false);
                    tutorialImages[tutorialIndex].gameObject.SetActive(true);
                }
            }
            //
            // switch (hasStarted)
            // {
            //     case false:
            //         if (isMoving) return;
            //         isMoving = true;
            //         quitSceneAction.action.Disable();
            //         mainCamera.transform.DOMove(cameraFinalPos1, firstTransitionDuration).SetEase(Ease.InOutSine)
            //             .OnComplete(() =>
            //             {
            //                 // Wait 4 seconds before starting Step 2
            //                 // Step 2: Wait, then move camera again and rotate
            //                 //DOVirtual.DelayedCall(secondTransitionDelay, () =>
            //                 //{
            //                 // Step 2: Move to cameraFinalPos2 AND rotate
            //                 Sequence cameraSequence = DOTween.Sequence();
            //                 cameraSequence.Append(mainCamera.transform.DOMove(cameraFinalPos2, secondTransitionDuration).SetEase(Ease.InOutSine));
            //                 cameraSequence.Join(mainCamera.transform.DORotate(cameraFinalRot, secondTransitionDuration).SetEase(Ease.InOutSine));
            //                 cameraSequence.OnComplete(() =>
            //                 {
            //                     // AsterEvents.Instance.OnGameStartComplete?.Invoke();
            //                     hasStarted= true;
            //                     tutorialIndex++;
            //                     tutorialImages[tutorialIndex].gameObject.SetActive(true);
            //                 });
            //                 //});
            //             });
            //         break;
            //     case true:
            //     {
            //         tutorialIndex++;
            //         if (tutorialIndex >= tutorialImages.Length)
            //         {
            //             tutorialImages[tutorialIndex - 1].gameObject.SetActive(false);
            //             tutorialIndex = -1;
            //             switchSceneAction.action.Disable();
            //             StartGameCompleted();
            //         }
            //         else
            //         {
            //             if(tutorialIndex-1 >= 0)
            //                 tutorialImages[tutorialIndex - 1].gameObject.SetActive(false);
            //             tutorialImages[tutorialIndex].gameObject.SetActive(true);
            //         }
            //
            //         break;
            //     }
            // }
        }

        private void StartGameCompleted()
        {
            hasStarted = true;
            isMoving   = true;
            GameEvents.OnGameStartComplete?.Invoke();
        }

        public void setupShouldBegin(bool shouldBegin)
        {
            setShouldBegin = shouldBegin;
        }
    }
}