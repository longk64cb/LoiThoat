using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Everest {
    public static class LocalData {
        private const string PREFIX = "Everest";
        private const int KEY_LENGTH = 15;
        private static readonly Dictionary<string, byte[]> cache = new();

        private static string GetKey<T>()
            => BaoMat.GetMD5(PREFIX + typeof(T).Name)[..KEY_LENGTH];

        private static string GetPath(string key)
            => Application.persistentDataPath + "/D" + key;//Thêm D để tránh bắt đầu bằng số

        internal static void Save<T>(T data, bool log = false) {
            string key = GetKey<T>();
            string json = JsonWrapper.ToJson(data);
            string finalString = DataChecker.GetFinalString(key, json);
            byte[] datas = BaoMat.MaHoa(finalString);
            cache[key] = datas;

            try {
                File.WriteAllBytes(GetPath(key), datas);
                var seq = DataChecker.IncAndSaveSequence(key);
                if (log) {
                    Debug.Log($"<color=#03A9F4>Saved {typeof(T).Name}</color> seq={seq}\n{json}");
                }
            } catch (Exception ex) {
                Debug.LogException(ex);
            }
        }

        internal static T Load<T>(T objectToOverwrite) where T : new() {
            string key = GetKey<T>();
            byte[] datas = null;
            bool fromFile = false;
            if (cache.ContainsKey(key)) {
                datas = cache[key];
            } else {
                try {
                    var path = GetPath(key);
                    if (File.Exists(path)) {
                        datas = File.ReadAllBytes(path);
                        fromFile = true;
                    }
                } catch (Exception ex) {
                    Debug.LogException(ex);
                }
                if (datas == null || datas.Length <= 0) {
                    return NewT<T>();
                }
                cache[key] = datas;
            }
            string finalString = BaoMat.GiaiMa(datas);
            var dataChecker = new DataChecker(key, finalString);
            if (fromFile) {
                if (dataChecker.SequenceNotOk() || dataChecker.DUIDNotOk()) {
                    cache.Remove(key);
                    return NewT<T>();
                }
            }
            var json = dataChecker.json;
            return JsonWrapper.FromJsonOverwrite(json, objectToOverwrite);
        }

        internal static T NewT<T>() where T : new() {
            //Mẹo để các biến class, List, Array phía trong không bị null
            return JsonWrapper.FromJsonOverwrite("{}", new T());
        }

        public static IEnumerable<Type> GetAllBaseData() {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                if (assembly.GetName().Name == "Assembly-CSharp") {
                    foreach (var type in assembly.GetTypes()) {
                        if (!type.IsAbstract && type.BaseType.IsGenericType
                            && type.BaseType.GetGenericTypeDefinition() == typeof(BaseData<>)) {
                            yield return type;
                        }
                    }
                }
            }
        }
    }
}
