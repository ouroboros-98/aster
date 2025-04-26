using System;
using Aster.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aster.Towers
{
    public class TowerParent : AsterSingleton<TowerParent>
    {
        private void Awake()
        {
            transform.position = Vector3.zero;
            gameObject.name    = "TOWERS";
        }

        private void Start()
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        }
    }
}