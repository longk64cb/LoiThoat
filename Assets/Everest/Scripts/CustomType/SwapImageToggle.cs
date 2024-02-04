using System;
using UnityEngine;
using UnityEngine.UI;

namespace Everest {
    [RequireComponent(typeof(Button))]
    public class SwapImageToggle : MonoBehaviour {
        public bool isOn = true;
        public Sprite onImage;
        public Sprite offImage;
        public Action<bool> onValueChanged;

        private Button button;

        private void Awake() {
            button = GetComponent<Button>();
            button.image.sprite = isOn ? onImage : offImage;
            button.onClick.AddListener(OnClick);
        }

        private void OnClick() {
            isOn = !isOn;
            button.image.sprite = isOn ? onImage : offImage;
            onValueChanged?.Invoke(isOn);
        }
    }
}