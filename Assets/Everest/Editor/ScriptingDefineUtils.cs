using System.Collections.Generic;
using UnityEditor;

namespace Everest {
    public static class ScriptingDefineUtils {
        private static readonly BuildTargetGroup[] allPlatform = { BuildTargetGroup.Standalone, BuildTargetGroup.Android, BuildTargetGroup.iOS };

        /// <summary>
        /// Adds the scripting define symbols on the platform if it doesn't exist.
        /// </summary>
        /// <param name="symbol">Symbol.</param>
        /// <param name="Platform">Platform.</param>
        public static void AddDefine(string symbol) {
            foreach (var platform in allPlatform) {
                string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
                List<string> symbols = new List<string>(symbolStr.Split(';'));
                if (!symbols.Contains(symbol)) {
                    symbols.Add(symbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, string.Join(";", symbols));
                }
            }
        }

        /// <summary>
        /// Removes the scripting define symbol on the platform if it exists.
        /// </summary>
        /// <param name="symbol">Symbol.</param>
        /// <param name="Platform">Platform.</param>
        public static void RemoveDefine(string symbol) {
            foreach (var platform in allPlatform) {
                string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
                List<string> symbols = new List<string>(symbolStr.Split(';'));
                if (symbols.Contains(symbol)) {
                    symbols.Remove(symbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, string.Join(";", symbols));
                }
            }
        }
    }
}
