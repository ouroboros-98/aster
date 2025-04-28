using System.Runtime.InteropServices;
using Aster.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aster.Core
{
    public class GameManager : AsterSingleton<GameManager>
    {
        [SerializeField] private AsterConfiguration _configuration;

        public AsterConfiguration Configuration => _configuration;

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