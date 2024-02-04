#if USE_NEWTONSOFT_JSON
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UnityEngine;

namespace Everest {
    public class JsonWrapper {
        private static readonly JsonUnityConverter converter = new JsonUnityConverter();
        public static T FromJson<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json, converter);
        }
        public static string ToJson(object obj) {
            return JsonConvert.SerializeObject(obj, Formatting.None, converter);
        }
    }

    public class JsonUnityConverter : JsonConverter {
        private static readonly Type[] supportedTypes = new Type[] {
            typeof(Color),
            typeof(Color32),
            typeof(Matrix4x4),
            typeof(Quaternion),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Vector2Int),
            typeof(Vector3Int)
        };

        public override bool CanConvert(Type objectType) {
            return supportedTypes.Contains(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) {
                return null;
            } else if (reader.TokenType == JsonToken.StartObject) {
                JObject jObject = JObject.Load(reader);
                if (objectType == typeof(Vector2Int)) {
                    return new Vector2Int((int)jObject.GetValue("x"), (int)jObject.GetValue("y"));
                } else if (objectType == typeof(Vector3Int)) {
                    return new Vector3Int((int)jObject.GetValue("x"), (int)jObject.GetValue("y"), (int)jObject.GetValue("z"));
                } else {
                    return JsonUtility.FromJson(jObject.ToString(), objectType);
                }
            }
            throw new Exception($"Unexpected token or value when parsing Unity type. Token: {reader.TokenType}, Value: {reader.Value}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (value is Vector2Int v2i) {
                writer.WriteStartObject();
                WriteJson(writer, "x", v2i.x);
                WriteJson(writer, "y", v2i.y);
                writer.WriteEndObject();
            } else if (value is Vector3Int v3i) {
                writer.WriteStartObject();
                WriteJson(writer, "x", v3i.x);
                WriteJson(writer, "y", v3i.y);
                WriteJson(writer, "z", v3i.z);
                writer.WriteEndObject();
            } else {
                //Phải dùng JObject không sẽ bị nhận là string
                JObject.Parse(JsonUtility.ToJson(value)).WriteTo(writer);
            }
        }

        private void WriteJson(JsonWriter writer, string name, int value) {
            writer.WritePropertyName(name);
            writer.WriteValue(value.ToString());
        }
    }
}
#else
using UnityEngine;

namespace Everest {
    internal class JsonWrapper {
        public static T FromJsonOverwrite<T>(string json, T objectToOverwrite) {
            if (objectToOverwrite == null) {
                return JsonUtility.FromJson<T>(json);
            } else {
                JsonUtility.FromJsonOverwrite(json, objectToOverwrite);
                return objectToOverwrite;
            }
        }

        public static string ToJson(object obj, bool prettyPrint = false) {
            return JsonUtility.ToJson(obj, prettyPrint);
        }
    }
}
#endif