using System;

namespace Everest.CustomEditor {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : Attribute {
        public string Text { get; private set; }
        public ButtonAttribute(string text = null) {
            Text = text;
        }
    }
}
