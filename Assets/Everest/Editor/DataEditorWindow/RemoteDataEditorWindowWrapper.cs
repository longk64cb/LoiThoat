using UnityEditor;

namespace Everest {
    public class RemoteDataEditorWindowWrapper : EditorWindow {
        private readonly BaseEditor.RemoteDataEditorWindow window = new();

        private void CreateGUI() {
            window.Init(typeof(BaseData<>), rootVisualElement);
        }

        private void OnDestroy() {
            window.OnDestroy();
        }
    }
}