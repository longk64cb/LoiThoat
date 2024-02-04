using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Everest {
    public class BaseDataStreamServer : MonoBehaviour {
        public const ushort PORT = 8282;

        private readonly GUIStyle textStyle = new();
        private HttpListener listener;
        private string serverInfo = null;
        private bool runServer = true;
        private Dictionary<string, Type> allDataType;

        private void Start() {
            allDataType = LocalData.GetAllBaseData().ToDictionary(x => x.Name);
            if (allDataType == null || allDataType.Count == 0) {
                return;
            }

            textStyle.fontStyle = FontStyle.Bold;
            textStyle.normal.textColor = Color.red;
            textStyle.fontSize = 25;

            listener = new HttpListener();
            listener.Prefixes.Add($"http://*:{PORT}/");
            listener.Start();

            //https://stackoverflow.com/questions/6803073/get-local-ip-address
            using (Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                serverInfo = "Data host: " + endPoint.Address.ToString();
            }

            HandleIncomingConnections();
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy() {
            runServer = false;
        }

        private void OnGUI() {
            if (serverInfo != null) {
                GUI.Label(new Rect(15, 50, 300, 25), serverInfo, textStyle);
            }
        }

        //https://gist.github.com/define-private-public/d05bc52dd0bed1c4699d49e2737e80e7
        public async void HandleIncomingConnections() {
            while (runServer) {
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                Debug.Log(req.Url.ToString());
                var result = "Error";
                if (req.Url.AbsolutePath != null && req.Url.AbsolutePath.Length > 1) {
                    string className = req.Url.AbsolutePath[1..];
                    allDataType.TryGetValue(className, out var clazz);
                    if (req.HttpMethod == "POST" && req.HasEntityBody) {
                        try {
                            using var reader = new StreamReader(req.InputStream, req.ContentEncoding);
                            var json = reader.ReadToEnd();
                            var obj = JsonUtility.FromJson(json, clazz);
                            var saveMethod = clazz.GetMethod("Save");
                            saveMethod.Invoke(obj, null);
                            result = "Successfully";
                        } catch (Exception ex) {
                            result = ex.Message;
                        }
                    } else if (req.HttpMethod == "GET") {
                        try {
                            var loadMethod = clazz.BaseType.GetMethod("Load");
                            var obj = loadMethod.Invoke(null, null);
                            result = JsonUtility.ToJson(obj);
                        } catch (Exception ex) {
                            result = ex.Message;
                        }
                    }
                }

                // Write the response 
                byte[] resultData = Encoding.UTF8.GetBytes(result);
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = resultData.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(resultData, 0, resultData.Length);
                resp.Close();
            }
        }
    }
}