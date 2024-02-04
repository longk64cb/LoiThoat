using System;
using UnityEditor;
using UnityEngine;

namespace Everest.CustomEditor {
    [CustomPropertyDrawer(typeof(EnumResource<,>))]
    public class EnumResourceDrawer : PropertyDrawer {
        //https://nosuchstudio.medium.com/learn-unity-editor-scripting-property-drawers-part-2-6fe6097f1586

        private float OneLine => EditorGUIUtility.singleLineHeight;
        private float Space => EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            Rect rectFoldout = new(position.min.x, position.min.y, position.size.x, OneLine);
            property.isExpanded = EditorGUI.Foldout(rectFoldout, property.isExpanded, label);
            if (property.isExpanded) {
                EditorGUI.indentLevel++;
                var itemsSP = property.FindPropertyRelative("items");
                var itemType = fieldInfo.FieldType.GenericTypeArguments[0];
                var itemValues = Enum.GetNames(itemType);
                itemsSP.arraySize = itemValues.Length;

                var previousPropertyHeight = OneLine + Space;
                var rect = new Rect(position.x, position.y, position.size.x, 0);

                for (int i = 0; i < itemValues.Length; i++) {
                    rect.y += previousPropertyHeight + Space;
                    float height = EditorGUI.GetPropertyHeight(itemsSP.GetArrayElementAtIndex(i));
                    rect.size = new Vector2(position.size.x, height);
                    var itemLabel = new GUIContent(itemValues[i]);
                    EditorGUI.PropertyField(rect, itemsSP.GetArrayElementAtIndex(i), itemLabel, true);
                    previousPropertyHeight = height;
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float height = OneLine + Space;
            if (property.isExpanded) {
                var itemsSP = property.FindPropertyRelative("items");
                for (int i = 0; i < itemsSP.arraySize; i++) {
                    height += EditorGUI.GetPropertyHeight(itemsSP.GetArrayElementAtIndex(i)) + Space;
                }
            }
            return height;
        }
    }
}