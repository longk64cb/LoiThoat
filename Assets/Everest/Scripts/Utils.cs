using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Everest {
    public static class Utils {
        public static bool Outside(Vector2 point, Vector2 bottomLeft, Vector2 topRight) {
            return point.x > topRight.x || point.x < bottomLeft.x ||
                      point.y > topRight.y || point.y < bottomLeft.y;
        }

        /// <summary>
        /// Cho biết một mảng là null hoặc rỗng
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(IList list)
            => list == null || list.Count == 0;

        public static T GetRandom<T>(IList<T> list)
            => list[UnityEngine.Random.Range(0, list.Count)];

        public static T GetRandom<T>() where T : Enum {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(UnityEngine.Random.Range(0, v.Length));
        }

        /// <summary>
        /// Ngẫu nhiên theo phân phối chuẩn
        /// </summary>
        public static double RandomGaussian(double min, double max, double stdDev = 2f, System.Random rand = null) {
            //https://stackoverflow.com/questions/218060/random-gaussian-variables
            rand ??= new System.Random(); //reuse this if you are generating many
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal = min + (max - min) / 2.0 + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return Math.Clamp(randNormal, min, max);
        }

        /// <summary>
        /// Ngẫu nhiên theo phân phối chuẩn, luôn nhỏ hơn maxExclusive
        /// </summary>
        public static int RandomGaussian(int minInclusive, int maxExclusive, double stdDev = 1, System.Random rand = null) {
            var rndRaw = RandomGaussian((double)minInclusive, maxExclusive, stdDev, rand);
            return Math.Min((int)Math.Floor(rndRaw), maxExclusive - 1);
        }

        public static void DestroyAllChildren(Transform transform) {
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
        }

        //https://stackoverflow.com/questions/12231569/is-there-in-c-sharp-a-method-for-listt-like-resize-in-c-for-vectort
        public static void Resize<T>(this List<T> list, int size, T element = default) {
            int count = list.Count;
            if (size < count) {
                list.RemoveRange(size, count - size);
            } else if (size > count) {
                if (size > list.Capacity)   // Optimization
                    list.Capacity = size;
                list.AddRange(Enumerable.Repeat(element, size - count));
            }
        }

        public static IEnumerator WaitAll(this MonoBehaviour mono, params IEnumerator[] ienumerators) {
            return ienumerators.Select(mono.StartCoroutine).ToArray().GetEnumerator();
        }

        public static Vector2 RotateVector2(Vector2 v, float degrees) {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public static float Clamp0360(float eulerAngles) {
            float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
            if (result < 0) {
                result += 360f;
            }
            return result;
        }

        private static readonly (long, string)[] abbreviations = new[] {
            (1_000_000_000_000, "T" ),
            (1_000_000_000, "B" ),
            (1_000_000, "M" ),
            (1_000,"K"),
        };

        public static string NumberToKMB(long number) {
            for (int i = 0; i < abbreviations.Length; i++) {
                (var n, var t) = abbreviations[i];
                if (Mathf.Abs(number) >= n * 10) {
                    long roundedNumber = number / n;
                    return roundedNumber.ToString() + t;
                }
            }
            return number.ToString();
        }

        private static readonly string[] suffixes = new string[] { "", "K", "M", "B", "T", "AA", "BB", "CC", "DD", "EE" };
        public static string NumberToKMBWithDot(float n) {
            if (n <= 0) return "0";
            var e = Mathf.FloorToInt(Mathf.Log10(n));
            e = Mathf.Clamp(e / 3, 0, suffixes.Length - 1);
            return (n / Mathf.Pow(10, e * 3)).ToString("0.##") + suffixes[e];
        }

        public static byte[] GetBytes(int i) {
            var r = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(r);
            }
            return r;
        }

        public static int GetInt(byte[] bytes) {
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(bytes);
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        //https://stackoverflow.com/questions/6435099/how-to-get-datetime-from-the-internet
        public const string TIME_HOST = "https://www.google.com";
        public static DateTime internetTimeResult;
        public static IEnumerator GetTimeFromInternet() {
            internetTimeResult = DateTime.MinValue;
            var task = Task.Run(() => {
                try {
                    var request = (HttpWebRequest)WebRequest.Create(TIME_HOST);
                    request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    var response = request.GetResponse();
                    string todaysDates = response.Headers["date"];
                    return DateTime.ParseExact(todaysDates,
                                               "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                               CultureInfo.InvariantCulture.DateTimeFormat,
                                               DateTimeStyles.AssumeUniversal);
                } catch (Exception ex) {
                    Debug.LogException(ex);
                    return DateTime.MinValue;
                }
            });
            yield return new WaitUntil(() => task.IsCompleted);
            internetTimeResult = task.Result;
        }

        public const string LOCATION_HOST = "http://ip-api.com/json?fields=query,status,countryCode,regionName,city,timezone,offset,currency&lang=en";
        public static IpAndLocationData ipAndLocationResult = null;
        public static IEnumerator GetIpAndLocation() {
            using UnityWebRequest www = UnityWebRequest.Get(LOCATION_HOST);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success) {
                Debug.Log(www.downloadHandler.text);
                IpAndLocationData respone = JsonUtility.FromJson<IpAndLocationData>(www.downloadHandler.text);
                if (respone.status == "success") {
                    ipAndLocationResult = respone;
                    ipAndLocationResult.ip = ipAndLocationResult.query;
                } else {
                    Debug.LogError("GetLocation error" + www.downloadHandler.text);
                }
            } else {
                Debug.LogError(www.error);
            }
        }
    }

    public class IpAndLocationData {
        public string query;//8.8.8.8
        public string ip;//8.8.8.8
        public string status;//success
        public string countryCode;//US
        public string regionName;//Virginia
        public string city;//Ashburn
        public string timezone;//America/New_York
        public int offset;//18000
        public string currency;//USD        
    }
}