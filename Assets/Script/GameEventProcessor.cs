using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace GDP2024
{
    public class GameEventProcessor : MonoBehaviour
    {
        [Inject] private ISubscriber<GameEvent> m_GameEventSubscriber;
        private IDisposable m_Subscription;

        private bool m_IsEnded;

        private void OnEnable()
        {
            m_Subscription = m_GameEventSubscriber.Subscribe((gameEvent) =>
            {
                if (m_IsEnded)
                    return;

                switch (gameEvent)
                {
                    case GameEvent.TEST:
                        Debug.Log("Receive test from web");
                        break;
                }
            });
        }

        private void Update()
        {

        }


        private void OnDisable()
        {
            m_Subscription.Dispose();
        }

    }
}