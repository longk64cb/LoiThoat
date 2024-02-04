using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Everest.CustomEditor {

    [UnityEditor.CustomEditor(typeof(ScriptableObject), true), CanEditMultipleObjects]
    public class EverestCustomEditorScriptableObject : EverestCustomEditor {
    }

    [UnityEditor.CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class EverestCustomEditor : Editor {
        private IEnumerable<FieldInfo> dasFields;
        private IEnumerable<PropertyInfo> dasProperties;
        private IEnumerable<MethodInfo> bMethods;
        private readonly Dictionary<string, bool> foldoutDasSaveState = new Dictionary<string, bool>();

        protected virtual void OnEnable() {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            dasFields = target.GetType().GetFields(bindingFlags)
                .Where(f => f.GetCustomAttributes(typeof(DisplayAsStringAttribute), true).Length > 0);
            dasProperties = target.GetType().GetProperties(bindingFlags)
                .Where(f => f.GetCustomAttributes(typeof(DisplayAsStringAttribute), true).Length > 0);
            bMethods = target.GetType().GetMethods(bindingFlags)
                .Where(f => f.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);
        }

        public override void OnInspectorGUI() {
            DrawDisplayAsString();
            if (dasFields.Any() || dasProperties.Any()) {
                EditorGUILayout.Space();
            }
            DrawButtons();
            if (bMethods.Any()) {
                EditorGUILayout.Space();
            }
            base.OnInspectorGUI();
        }

        private void DrawDisplayAsString() {
            foreach (var f in dasFields) {
                object value = f.GetValue(target);
                DrawDisplayAsString(f.Name, value);
            }
            foreach (var f in dasProperties) {
                if (f.CanRead) {
                    object value = f.GetValue(target);
                    DrawDisplayAsString(f.Name, value);
                }
            }
        }

        private void DrawDisplayAsString(string name, object value) {
            if (value == null) {
                EditorGUILayout.LabelField(name, "null");
            } else if (value is Object objectValue) {
                using (new EditorGUI.DisabledScope(true)) {
                    EditorGUILayout.ObjectField(name, objectValue, value.GetType(), true);
                }
            } else {
                if (value is IEnumerable) {
                    foldoutDasSaveState.TryGetValue(name, out bool foldoutValue);
                    foldoutDasSaveState[name] = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutValue, name);
                    if (foldoutDasSaveState[name]) {
                        EditorGUI.indentLevel++;
                        if (value is IDictionary) {
                            foreach (DictionaryEntry item in value as IDictionary) {
                                DrawDisplayAsString(item.Key.ToString(), item.Value);
                            }
                        } else {
                            int idx = 0;
                            foreach (var item in value as IEnumerable) {
                                DrawDisplayAsString(idx.ToString(), item);
                                idx++;
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                } else {
                    EditorGUILayout.LabelField(name, value.ToString());
                }
            }
        }

        protected void DrawButtons() {
            foreach (var method in bMethods) {
                Button(serializedObject.targetObject, method);
            }
        }

        private static void Button(Object target, MethodInfo methodInfo) {
            if (methodInfo.GetParameters().All(p => p.IsOptional)) {
                ButtonAttribute buttonAttribute = (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                string buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? ObjectNames.NicifyVariableName(methodInfo.Name) : buttonAttribute.Text;
                if (GUILayout.Button(buttonText)) {
                    object[] defaultParams = methodInfo.GetParameters().Select(p => p.DefaultValue).ToArray();
                    IEnumerator resultIEnumerator = methodInfo.Invoke(target, defaultParams) as IEnumerator;
                    if (Application.isPlaying && resultIEnumerator != null && target is MonoBehaviour behaviour) {
                        behaviour.StartCoroutine(resultIEnumerator);
                    }
                }
            } else {
                string warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
            }
        }
    }
}