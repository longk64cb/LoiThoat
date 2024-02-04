using System;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace Everest.CustomEditor {
    [CustomPropertyDrawer(typeof(DisplayAsDateTimeAttribute))]
    public class DisplayAsDateTimeDrawer : PropertyDrawer {
        private string lastGUIInput = null;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType == SerializedPropertyType.Integer) {
                var format = (attribute as DisplayAsDateTimeAttribute).format;
                var value = property.longValue;
                var timeString = new DateTime(value).ToString(format, CultureInfo.InvariantCulture);
                label.tooltip += $"{value}";
                var input = EditorGUI.DelayedTextField(position, label, timeString);
                if (input != lastGUIInput) {
                    lastGUIInput = input;
                    if (string.IsNullOrWhiteSpace(input)) {
                        property.longValue = DateTime.Now.Ticks;
                    } else {
                        try {
                            var parseValue = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture);
                            property.longValue = parseValue.Ticks;
                        } catch (FormatException ex) {
                            Debug.LogException(ex);
                        }
                    }
                }
            } else {
                EditorGUI.LabelField(position, label.text, "Use DisplayAsDateTime with long value");
            }
        }
    }
}