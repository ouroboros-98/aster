using Aster.Utils;
using UnityEngine;

namespace Aster.Core
{
    public class AsterMono : MonoBehaviour
    {
        private AsterEvents _events;

        protected AsterEvents GameEvents
        {
            get
            {
                if (_events == null) _events = AsterEvents.Instance;

                return _events;
            }
        }

        [SerializeField, HideInNormalInspector] private bool debug = false;

        protected void ValidateRawComponent<T>(ref T component,        bool self     = true, bool parents = false,
                                               bool  children = false, bool optional = false)
        {
            if (component == null)
            {
                component = GetComponent<T>();
                if (self     && component == null) component = GetComponent<T>();
                if (parents  && component == null) component = GetComponentInParent<T>();
                if (children && component == null) component = GetComponentInChildren<T>();

                if (component == null && !optional)
                {
                    Debug.LogError($"Missing component of type {typeof(T)} on {gameObject.name}");
                }
            }
        }

        protected void ValidateComponent<T>(ref T component,        bool self     = true, bool parents = false,
                                            bool  children = false, bool optional = false)
            where T : Component
        {
            ValidateRawComponent<T>(ref component, self, parents, children, optional);
        }

        protected void debugPrint(object message)
        {
            if (!debug) return;

            print($"[{gameObject.name}::{this.GetType().Name}] " + message);
        }

        #region HELPER FUNCTIONS

        protected bool IsNull(object obj)
        {
            if (obj == null) return true;

            if (obj is GameObject gameObject)
                return gameObject == null;

            if (obj is Component component)
                return component == null;

            return false;
        }

        protected bool IsNotNull(object obj)
        {
            if (obj == null) return false;

            if (obj is GameObject gameObject)
                return gameObject != null;

            if (obj is Component component)
                return component != null;

            return true;
        }

        #endregion
    }
}