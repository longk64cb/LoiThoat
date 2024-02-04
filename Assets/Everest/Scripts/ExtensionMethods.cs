using System;
using System.Collections;
using UnityEngine;

namespace Everest {
    public class DisposableCoroutine : IDisposable {
        private readonly MonoBehaviour target;
        private readonly Coroutine routine;

        public DisposableCoroutine(MonoBehaviour target, Coroutine routine) {
            this.target = target;
            this.routine = routine;
        }

        public void Dispose() {
            if (routine != null) target.StopCoroutine(routine);
        }
    }

    public static class ExtensionMethods {

        /// <summary>
        /// Dừng chạy khi gameObject bị disable, MonoBehaviour bị disable không ảnh hưởng
        /// </summary>
        public static IDisposable Delay<T>(this T t, float timeSec, Action action, bool ignoreTimeScale = false) where T : MonoBehaviour {
            if (action == null || !t.gameObject.activeInHierarchy) return null;
            if (timeSec < 0) {
                return null;//Không làm gì cả khi thời gian < 0
            } else if (Mathf.Approximately(timeSec, 0)) {
                action();//Thực hiện ngay lập tức nếu thời gian bằng 0
                return null;
            } else {
                return new DisposableCoroutine(t, t.StartCoroutine(DelayCoroutine(timeSec, action, ignoreTimeScale)));
            }
        }

        private static IEnumerator DelayCoroutine(float timeSec, Action action, bool ignoreTimeScale = false) {
            if (ignoreTimeScale) {
                yield return new WaitForSecondsRealtime(timeSec);
            } else {
                yield return new WaitForSeconds(timeSec);
            }
            action();
        }

        public static IDisposable DoActionEveryTime<T>(this T t, float timeSec, int count, Action action, bool doOnStart = false, bool ignoreTimeScale = false) where T : MonoBehaviour {
            return t.DoActionEveryTime(timeSec, count, action, null, doOnStart, ignoreTimeScale);
        }
        public static IDisposable DoActionEveryTime<T>(this T t, float timeSec, Action action, Action compeleteAction, bool doOnStart = false, bool ignoreTimeScale = false) where T : MonoBehaviour {
            return t.DoActionEveryTime(timeSec, -1, action, compeleteAction, doOnStart, ignoreTimeScale);
        }
        public static IDisposable DoActionEveryTime<T>(this T t, float timeSec, Action action, bool doOnStart = false, bool ignoreTimeScale = false) where T : MonoBehaviour {
            return t.DoActionEveryTime(timeSec, -1, action, null, doOnStart, ignoreTimeScale);
        }
        public static IDisposable DoActionEveryTime<T>(this T t, float timeSec, int count, Action action, Action compeleteAction, bool doOnStart = false, bool ignoreTimeScale = false) where T : MonoBehaviour {
            if (action == null || !t.gameObject.activeInHierarchy) return null;
            if (Mathf.Approximately(timeSec, 0) && count > 0) {
                for (int i = 0; i < count; i++) action();
                compeleteAction?.Invoke();
                return null;
            } else if (timeSec > 0) {
                if (doOnStart) {
                    count--;
                    action();
                }
                return new DisposableCoroutine(t, t.StartCoroutine(DoActionEveryTimeCoroutine(timeSec, count, action, compeleteAction, ignoreTimeScale)));
            } else {
                //Không làm gì cả nếu timeSec<0 hoặc timeSec==0 mà count<=0
                return null;
            }
        }

        private static IEnumerator DoActionEveryTimeCoroutine(float timeSec, int count, Action action, Action compeleteAction, bool ignoreTimeScale = false) {
            //Nếu count<0 sẽ lặp vô tận
            while (count != 0) {
                if (ignoreTimeScale) {
                    yield return new WaitForSecondsRealtime(timeSec);
                } else {
                    yield return new WaitForSeconds(timeSec);
                }
                if (count > 0) count--;
                action();
            }
            compeleteAction?.Invoke();
        }
    }
}