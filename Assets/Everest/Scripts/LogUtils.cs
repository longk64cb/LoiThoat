using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Everest {
    public static class LogUtils {
        public const string defaultColor = "#03A9F4";
        public static void Log(string msg, string color = defaultColor) {
            Debug.Log($"<color={color}>{msg}</color>");
        }
        public static void LogWarning(string msg, string color = defaultColor) {
            Debug.LogWarning($"<color={color}>{msg}</color>");
        }
        public static void LogError(string msg, string color = defaultColor) {
            Debug.LogError($"<color={color}>{msg}</color>");
        }

        public static string GetTextWithColor(string text, string color)
            => $"<color={color}>{text}</color>";

        public static void DrawPolyLine(IEnumerable<Vector3> points, Color color, float duration) {
#if UNITY_EDITOR
            int i = 0;
            Vector3 start = Vector3.zero;
            foreach (var point in points) {
                if (i == 0) {
                    start = point;
                } else {
                    var delta = point - start;
                    Debug.DrawLine(start, point, color, duration);
                    Debug.DrawRay(start, delta / 10, Color.white, duration);
                    start = point;
                }
                i++;
            }
            if (i <= 1) {
                Debug.LogError("DebugDrawLine points.Count <= 1");
            }
#endif
        }

        public static string GetString<T>(IEnumerable<T> input) {
            if (input == null) return "null";
            return string.Join(", ", input.Select(x => x.ToString()));
        }
    }
}
