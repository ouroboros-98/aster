using UnityEngine;

namespace Aster.Utils.Attributes
{
    /// <summary>
    /// Attribute that causes a serializable class to be drawn inside a box with always expanded properties.
    /// </summary>
    public class BoxedPropertyAttribute : PropertyAttribute
    {
        public float Padding { get; private set; }
        
        public BoxedPropertyAttribute(float padding = 5f)
        {
            Padding = padding;
        }
    }
}
