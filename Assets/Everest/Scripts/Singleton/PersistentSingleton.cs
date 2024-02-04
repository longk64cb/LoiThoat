using UnityEngine;

namespace Everest {
    public abstract class PersistentSingleton<T> : MonoBehaviour where T : Component, ICanInit {
        protected static T _instance;
        protected static bool initialized = false;

        public static T IRaw => _instance;

        private static void CheckAndInit() {
            if (!initialized && _instance != null) {
                initialized = true;
                _instance.Init();
            }
        }

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
                CheckAndInit();
                return _instance;
            }
        }

        protected virtual void Awake() {
            T @this = this as T;
            if (_instance != null && _instance != @this) {
                Destroy(gameObject);
            } else {
                _instance = @this;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                CheckAndInit();
            }
        }

        protected virtual void OnDestroy() {
            initialized = false;
        }
    }

    public interface ICanInit {
        void Init();
    }
}
