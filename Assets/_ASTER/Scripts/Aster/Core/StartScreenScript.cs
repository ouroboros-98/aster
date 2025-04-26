using System;
using Aster.Core;
using Aster.Core.InputSystem;
using DependencyInjection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreenScript : MonoBehaviour
{
    private InputHandler        inputHandler;

    public void Awake()
    {
        inputHandler = new InputHandler();
    }

    private void OnEnable()
    {
        // Subscribe to events from InputHandler
        if (inputHandler != null)
        {
            inputHandler.OnSelectTower += HandleButton;
        }
    }

    private void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnSelectTower -= HandleButton;
        }
    }

    private void HandleButton()
    {
        SceneManager.LoadScene("MainScene");
    }
}