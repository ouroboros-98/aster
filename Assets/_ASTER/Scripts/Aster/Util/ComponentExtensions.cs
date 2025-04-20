using UnityEngine;

namespace Aster.Utils
{
    public static class ComponentExtensions
    {
        public static bool ScanForComponent<T>(this Component component,
                                              out  T         outComponent,
                                              bool           self     = true,
                                              bool           parents  = false,
                                              bool           children = false) where T : Component
        {
            if (component.TryGetComponent(out outComponent)) return true;
            if (parents)
            {
                outComponent = component.transform.GetComponentInParent<T>();
                if (outComponent != null) return true;
            }

            if (children)
            {
                outComponent = component.transform.GetComponentInChildren<T>();
                if (outComponent != null) return true;
            }

            return false;
        }
    }
}