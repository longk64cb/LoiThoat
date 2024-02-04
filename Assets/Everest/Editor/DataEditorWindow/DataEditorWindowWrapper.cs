using UnityEditor;

namespace Everest {
    public class DataEditorWindowWrapper : EditorWindow {
        private readonly BaseEditor.DataEditorWindow window = new();

        private void CreateGUI() {
            window.Init(typeof(BaseData<>), rootVisualElement);
        }

        private void OnGUI() {
            window.OnGUI();
        }
    }
}