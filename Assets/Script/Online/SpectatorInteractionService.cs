#if SECRET_VERSION

using NativeWebSocket;
using System;
using UnityEngine;
using VContainer.Unity;

namespace GDP2024
{
    public class SpectatorInteractionService : IStartable, ITickable, IDisposable
    {
        private const string kAddress = "wss://gdp2024-instance.multiplayertournamentonline.fr";

        private WebSocket m_Websocket;
        public bool IsConnected { get; private set; } = false;
        public bool IsInitialized { get; private set; } = false;
        public Action<string> OnMessage;

        public async void Start()
        {
            m_Websocket = new WebSocket(kAddress);
            m_Websocket.OnOpen += () =>
            {
                IsConnected = true;
                Debug.Log("Connection open!");
            };

            m_Websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            m_Websocket.OnClose += (e) =>
            {
                IsConnected = false;
                Debug.Log("Connection closed!");
            };

            m_Websocket.OnMessage += (bytes) =>
            {
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                OnMessage?.Invoke(message);
            };

            IsInitialized = true;

            // Waiting for messages
            await m_Websocket.Connect();
        }

        public async void Send(string[] messages)
        {
            string x = "unity-";
            for(int i = 0; i < messages.Length; i++)
            {
                x += messages[i];
                if(i < messages.Length - 1)
                {
                    x += "-";
                }
            }

            var values = new Dictionary<string, string>
              {
                  { "message", x },
              };
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(postAddress, content);
        }

        public void Tick()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            m_Websocket.DispatchMessageQueue();
#endif
        }

        public async void Dispose()
        {
            await m_Websocket.Close();
        }
    }
}
#endif