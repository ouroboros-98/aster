using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using _ASTER.Prefabs.Core;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty switchSceneAction;

    [SerializeField]
    private InputActionProperty switchCancel;

    [SerializeField]
    private float cooldownSeconds = 4f;

    [SerializeField]
    private string nextSceneName = "MainSceneReplay";

    [SerializeField]
    private string cancelSceneName = "MainSceneStart";

    [SerializeField]
    private string mainSceneName = "MainScene";

    private bool canSwitch;

    private void Awake()
    {
        // Start countdown immediately on awake.
        StartCoroutine(CooldownRoutine());
    }

    private void OnEnable()
    {
        switchSceneAction.action.performed += OnSwitchScenePressed;
        switchCancel.action.performed      += OnSwitchSceneCancel;
        switchSceneAction.action.Enable();
        switchCancel.action.Enable();
    }

    private void OnDisable()
    {
        switchSceneAction.action.performed -= OnSwitchScenePressed;
        switchCancel.action.performed      -= OnSwitchSceneCancel;
        switchSceneAction.action.Disable();
        switchCancel.action.Disable();
    }

    private void OnSwitchScenePressed(InputAction.CallbackContext ctx)
    {
        if (canSwitch)
        {
            StartSceneManager.FirstTime = false;
            SceneManager.LoadScene(mainSceneName);
            // Optionally, restart cooldown if needed.

            canSwitch = false;
        }
    }

    private void OnSwitchSceneCancel(InputAction.CallbackContext ctx)
    {
        if (canSwitch)
        {
            StartSceneManager.FirstTime = true;
            SceneManager.LoadScene(mainSceneName);
            // Optionally, restart cooldown if needed.

            canSwitch = false;
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldownSeconds);
        canSwitch = true;
    }
}