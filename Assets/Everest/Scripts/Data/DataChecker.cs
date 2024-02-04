using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Everest {
    internal class DataChecker {
        private const int DUID_LENGTH = 10;
        private const int SEQ_LENGTH = 15;

        private static readonly string myDUID;

        static DataChecker() {
            string deviceUniqueIdentifier = "unsupported";
            if (SystemInfo.unsupportedIdentifier != SystemInfo.deviceUniqueIdentifier) {
                deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
            }
            myDUID = deviceUniqueIdentifier.PadRight(DUID_LENGTH, 'x')[..DUID_LENGTH];
            LoadSequence();
        }

        public static readonly string SEQUENCE_FILE_PATH = Application.persistentDataPath + "/sequence";
        private static readonly Dictionary<string, long> seqDic = new();

        public readonly string key;
        public readonly string finalString;
        public readonly long seq;
        public readonly string duid;
        public readonly string json;

        public DataChecker(string key, string finalString) {
            this.key = key;
            this.finalString = finalString;
            seq = long.Parse(finalString[..SEQ_LENGTH]);
            duid = finalString.Substring(SEQ_LENGTH, DUID_LENGTH);
            json = finalString[(SEQ_LENGTH + DUID_LENGTH)..];
        }

        public static string GetFinalString(string key, string json) {
            if (!seqDic.ContainsKey(key)) {
                seqDic[key] = 0;
            }
            return (seqDic[key] + 1).ToString("D" + SEQ_LENGTH) + myDUID + json;
        }

        public static long IncAndSaveSequence(string key) {
            seqDic[key] += 1;

            StringBuilder sb = new();
            foreach (var item in seqDic) {
                sb.Append($"{item.Key};{item.Value};");
            }
            var datas = BaoMat.MaHoa(sb.ToString());
            try {
                File.WriteAllBytes(SEQUENCE_FILE_PATH, datas);
            } catch (Exception ex) {
                Debug.LogException(ex);
            }
            return seqDic[key];
        }

        private static void LoadSequence() {
            try {
                if (File.Exists(SEQUENCE_FILE_PATH)) {
                    var rawByte = File.ReadAllBytes(SEQUENCE_FILE_PATH);
                    var rawString = BaoMat.GiaiMa(rawByte);
                    var datas = rawString.Split(";", StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < datas.Length / 2; i++) {
                        string key = datas[i * 2];
                        long value = long.Parse(datas[i * 2 + 1]);
                        seqDic.Add(key, value);
                    }
                }
            } catch (Exception ex) {
                Debug.LogException(ex);
            }
        }

        public bool SequenceNotOk() {
            seqDic.TryGetValue(key, out var correctSeq);
            if (seq != correctSeq) {
                Debug.LogError($"Save file is corrupted: Sequence correct={correctSeq}, current={seq}");
                return true;
            }
            return false;
        }

        public bool DUIDNotOk() {
            if (duid != myDUID) {
                Debug.LogError($"Save file is corrupted: DUID correct={myDUID}, current={duid}");
                return true;
            }
            return false;
        }
    }
}
