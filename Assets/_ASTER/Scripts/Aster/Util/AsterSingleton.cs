using Aster.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aster.Utils
{
    /// <summary>
    /// A generic Singleton class for MonoBehaviours.
    /// Example usage: public class GameManager : MonoSingleton<GameManager>
    /// </summary>
    public class AsterSingleton<T> : AsterMono where T : AsterMono
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    var singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                    DontDestroyOnLoad(singletonObject); // Don't destroy the object when loading a new scene
                }

                return _instance;
            }
        }

        // Ensure no other instances can be created by having the constructor as protected
        protected AsterSingleton()
        {
        }

        protected void SetDestroyOnLoad() =>
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}