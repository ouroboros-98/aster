using UnityEngine;
using UnityEditor;
using Aster.Utils.Attributes;

namespace Aster.Utils.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(BoxedPropertyAttribute))]
    public class BoxedPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            // Draw a box around the entire property
            Rect boxRect = new Rect(position.x, position.y, position.width, position.height);
            GUI.Box(boxRect, GUIContent.none);
            
            // Add padding inside the box
            BoxedPropertyAttribute boxedAttr = attribute as BoxedPropertyAttribute;
            float padding = boxedAttr != null ? boxedAttr.Padding : 5f;
            position.x += padding;
            position.y += padding;
            position.width -= padding * 2;
            
            // Draw the label as header
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
            position.y += EditorGUIUtility.singleLineHeight;
            
            // Always draw properties
            EditorGUI.indentLevel++;
            
            // Draw each property
            SerializedProperty iterator = property.Copy();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                // Skip the first property (which is the script reference)
                if (SerializedProperty.EqualContents(iterator, property))
                    continue;
                
                // Stop when we reach the end of this property's children
                if (iterator.depth != property.depth + 1)
                    break;
                
                enterChildren = false;
                position.height = EditorGUI.GetPropertyHeight(iterator, true);
                EditorGUI.PropertyField(position, iterator, true);
                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            }
            
            EditorGUI.indentLevel--;
            
            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight; // Header
            
            // Get padding from attribute
            BoxedPropertyAttribute boxedAttr = attribute as BoxedPropertyAttribute;
            float padding = boxedAttr != null ? boxedAttr.Padding * 2 : 10f; // Total padding (top and bottom)
            
            // Always calculate full height
            SerializedProperty iterator = property.Copy();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                if (SerializedProperty.EqualContents(iterator, property))
                    continue;
                    
                if (iterator.depth != property.depth + 1)
                    break;
                    
                enterChildren = false;
                height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
            }
            
            return height + padding;
        }
    }
}
