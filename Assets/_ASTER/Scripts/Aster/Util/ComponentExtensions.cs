using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aster.Utils
{
    public static class ComponentExtensions
    {
        public static bool ScanForComponent<T>(this Component component,
                                               out  T         outComponent,
                                               bool           self     = true,
                                               bool           parents  = false,
                                               bool           children = false)
        {
            outComponent = default(T);

            if (self)
            {
                outComponent = component.GetComponent<T>();
                if (outComponent != null) return true;
            }

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

        public static bool ScanForComponents<T>(this Component component,
                                                out  T[]       outComponents,
                                                bool           self     = true,
                                                bool           parents  = false,
                                                bool           children = false)
        {
            HashSet<T> components = new();

            if (self)
            {
                T[] scan = component.GetComponents<T>();
                components.UnionWith(scan);
            }

            if (parents)
            {
                T[] scan = component.transform.GetComponentsInParent<T>();
                components.UnionWith(scan);
            }

            if (children)
            {
                T[] scan = component.transform.GetComponentsInChildren<T>();
                components.UnionWith(scan);
            }

            outComponents = components.ToArray();

            return components.Count > 0;
        }
    }
}