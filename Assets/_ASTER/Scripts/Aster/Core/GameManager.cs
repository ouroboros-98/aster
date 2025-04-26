using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aster.Core
{
    public class GameManager : AsterMono
    {
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
            SceneManager.LoadScene("DeathScreen");
        }
    }
}