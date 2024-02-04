using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Everest {
    internal class EverestMenu {
        [MenuItem("Everest/Take Screenshot %\\")]
        public static void Screenshot() {
            string name = "ss_" + System.Guid.NewGuid().ToString().Replace("-", "") + ".png";
            string dataPath = Application.dataPath[..^6];
            string folder = dataPath + "Screenshots/";
            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
            ScreenCapture.CaptureScreenshot("Screenshots/" + name);
            Debug.Log("Screenshot path: " + folder + name);
        }

        [MenuItem("Everest/PlayerPrefs/Delete PlayerPrefs")]
        public static void DeleteAllPlayerPrefs() {
            PlayerPrefs.DeleteAll();
            Debug.Log("Delete PlayerPrefs Complete");
        }

        [MenuItem("Everest/PlayerPrefs/Show PlayerPrefs")]
        public static void ShowAllPlayerPrefs() {
            PlayerPrefPair[] keyValues = PlayerPrefsExtension.GetAll();
            for (int i = 0; i < keyValues.Length; i++) {
                Debug.Log($"{keyValues[i].Key}={keyValues[i].Value}");
            }
        }

        [MenuItem("Everest/PlayerPrefs/Editor/Delete EditorPrefs")]
        public static void DeleteEditorPrefs() {
            EditorPrefs.DeleteAll();
            Debug.Log("Delete EditorPrefs Complete");
        }

        [MenuItem("Everest/Data Editor", false, 1)]
        public static void ShowDataEditor() {
            EditorWindow wnd = EditorWindow.GetWindow<DataEditorWindowWrapper>();
            wnd.titleContent = new GUIContent("Data Editor");
        }

        [MenuItem("Everest/Remote Data Editor", false, 1)]
        public static void ShowRemoteDataEditor() {
            EditorWindow wnd = EditorWindow.GetWindow<RemoteDataEditorWindowWrapper>();
            wnd.titleContent = new GUIContent("Remote Data Editor");
        }

        [MenuItem("Everest/Delete All Data", false, 2)]
        public static void DeleteAllData() {
            var okClick = EditorUtility.DisplayDialog("Delete All Data",
                "Are you sure you want delete all save game data?", "Yes", "No");
            if (okClick) {
                foreach (var dataType in LocalData.GetAllBaseData()) {
                    var deleteMethod = dataType.BaseType.GetMethod("Delete");
                    deleteMethod.Invoke(null, null);
                }
                Debug.Log("Delete all data completed");
            }
        }
    }
}
