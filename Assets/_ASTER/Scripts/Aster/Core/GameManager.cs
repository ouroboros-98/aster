using System.Runtime.InteropServices;
using UnityEngine;

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

        private void FinishGame(int hp)
        {
            Debug.Log("Game Over — Light Source Destroyed!");
            Application.Quit();
        }
    }
}