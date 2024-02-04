using UnityEngine;

namespace Everest {
    public class Singleton<T> : MonoBehaviour where T : Component {
        private static T _instance;

        public static T I {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<T>();
#if UNITY_EDITOR
                    if (Application.isPlaying) {
                        Debug.LogWarning("Run FindObjectOfType " + typeof(T).Name);
                    }
#endif
                }
                return _instance;
            }
        }

        protected virtual void Awake() {
            T @this = this as T;
            if (_instance != null && _instance != @this) {
                Destroy(gameObject);
                Debug.LogError($"2 instances of {typeof(T).Name} destroy 1");
            } else {
                _instance = @this;
            }
        }
    }
}
