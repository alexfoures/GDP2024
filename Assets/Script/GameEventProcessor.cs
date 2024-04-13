using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace GDP2024
{
    public class GameEventProcessor : MonoBehaviour
    {
        [Inject] private ISubscriber<GameEvent> m_GameEventSubscriber;
#if SECRET_VERSION
        [Inject] SpectatorInteractionService m_InteractionService;
#endif
        [SerializeField] private PowerUpCanvas m_PowerUpCanvas;
        [SerializeField] private Image timer;
        [SerializeField] private PowerUpManagement powerUpManagement;
        private IDisposable m_Subscription;

        [SerializeField] private float choiceTime;
        private bool inSelection = false;

        private int one = 0;
        private int two = 0;
        private int keep = 0;

        string[] values;

        private Dictionary<string, PowerUpManagement.PowerUp> mapping = new Dictionary<string, PowerUpManagement.PowerUp> {
            {"0", PowerUpManagement.PowerUp.Drill },
            {"1", PowerUpManagement.PowerUp.Resize },
            {"2", PowerUpManagement.PowerUp.DoubleJump },
            {"3", PowerUpManagement.PowerUp.Resize },
            {"4", PowerUpManagement.PowerUp.DoubleJump },
        };

        private void OnEnable()
        {
            m_Subscription = m_GameEventSubscriber.Subscribe((gameEvent) =>
            {
                if(inSelection)
                {
                    switch (gameEvent)
                    {
                        case GameEvent.ONE:
                            one++;
                            m_PowerUpCanvas.UpdateText(one, 0);
                            break;
                        case GameEvent.TWO:
                            two++;
                            m_PowerUpCanvas.UpdateText(two, 1);
                            break;
                        case GameEvent.KEEP:
                            keep++;
                            m_PowerUpCanvas.UpdateText(keep, 2);
                            break;
                    }
                }

            });

            SendPowerUps();
        }

        private void Update()
        {

        }


        private void OnDisable()
        {
            m_Subscription.Dispose();
        }

        private void SendPowerUps()
        {
            inSelection = true;
            one = 0;
            two = 0;
            keep = 0;
            values = m_PowerUpCanvas.nextStep();
#if SECRET_VERSION
            Debug.Log(values);
            m_InteractionService.Send(values);
#endif
            StartCoroutine(Timer());
        }

        IEnumerator Timer()
        {
            float counter = choiceTime;
            while (counter > 0)
            {
                yield return new WaitForEndOfFrame();
                counter -= Time.deltaTime;
                timer.fillAmount = counter / choiceTime;
            }
            inSelection = false;
            if(one > two && one > keep)
            {
                ApplyChange(0);
            }
            else if (two > one && two > keep)
            {
                ApplyChange(1);
            }
            else if(two == one && two > keep)
            {
                ApplyChange(UnityEngine.Random.Range(0, 2));
            }
            else if(two == one && one == keep)
            {
                ApplyChange(UnityEngine.Random.Range(0, 2));
            }
            else
            {
                ApplyChange(2);
            }
            m_PowerUpCanvas.stop();
        }

        void ApplyChange(int id)
        {
            powerUpManagement.powerUp = mapping[id.ToString()];
        }

    }
}