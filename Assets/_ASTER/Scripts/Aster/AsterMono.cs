using Aster.Utils;
using UnityEngine;

namespace Aster.Core
{
    public class AsterMono : MonoBehaviour
    {
        [SerializeField, HideInNormalInspector] private bool debug = false;

        protected void ValidateComponent<T>(ref T component,        bool self     = true, bool parents = false,
            bool                                  children = false, bool optional = false)
            where T : Component
        {
            if (component == null)
            {
                component = GetComponent<T>();
                if (self && component == null) component     = GetComponent<T>();
                if (parents && component == null) component  = GetComponentInParent<T>();
                if (children && component == null) component = GetComponentInChildren<T>();

                if (component == null && !optional)
                {
                    Debug.LogError($"Missing component of type {typeof(T)} on {gameObject.name}");
                }
            }
        }

        protected void debugPrint(object message)
        {
            if (!debug) return;

            print($"[{gameObject.name}::{this.GetType().Name}] " + message);
        }
    }
}