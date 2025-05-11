using System;
using System.Runtime.InteropServices;
using Aster.Utils;
using UnityEngine.SceneManagement;

namespace Aster.Core
{
    public class GameManager : AsterSingleton<GameManager>
    {
        public InteractionOwnershipManager InteractionOwnershipManager { get; private set; }

        private void Awake()
        {
            InteractionOwnershipManager = new();
        }

        private void OnEnable()
        {
            AsterEvents.Instance.OnLightSourceDestroyed += FinishGame;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnLightSourceDestroyed -= FinishGame;
        }

        private void FinishGame()
        {
//             Debug.Log("Game Over — Light Source Destroyed!");
// #if UNITY_EDITOR
//             UnityEditor.EditorApplication.isPlaying = false;
// #else
//                 Application.Quit();
// #endif
            //SceneManager.LoadScene("DeathScreen");
        }
    }
}