using System;
using System.IO;
using UnityEngine;

namespace Everest {
    public class BaseData<T> : ISaveable where T : class, new() {
        public DateTime? lastUpdateInPreviousSession = null;

        public virtual bool EnableLog => false;

        public virtual void Save() {
            if (lastUpdateInPreviousSession == null) {
                if (File.Exists(DataChecker.SEQUENCE_FILE_PATH)) {
                    lastUpdateInPreviousSession = File.GetLastWriteTimeUtc(DataChecker.SEQUENCE_FILE_PATH);
                } else {
                    lastUpdateInPreviousSession = DateTime.MinValue;
                }
            }
            cache = this as T;
            LocalData.Save(cache, EnableLog);
        }

        public static T Load() {
            try {
                cache = LocalData.Load(cache);
            } catch (Exception ex) {
                Debug.LogException(ex);
            }
            return cache;
        }

        public static T Delete() {
            cache = LocalData.NewT<T>();
            LocalData.Save(cache);
            return cache;
        }

        private static T cache;
        /// <summary>
        /// Chỉ dùng cache để hiển thị, để cập nhật số liệu cần gọi Load, tránh hack
        /// </summary>
        public static T Cache {
            get {
                if (cache == null) {
                    Load();
                }
                return cache;
            }
        }
    }

    public interface ISaveable {
        void Save();
    }
}