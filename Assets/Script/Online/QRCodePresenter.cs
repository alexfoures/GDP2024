#if SECRET_VERSION
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using VContainer;

namespace GDP2024
{
    public class QRCodePresenter : MonoBehaviour
    {
        private const int kGameSceneIndex = 1;

        [Inject] SpectatorInteractionService m_InteractionService;

        [SerializeField] private ParticleSystem m_LightbulbEmojiParticleSystem;
        [SerializeField] private ParticleSystem m_HeartEmojiParticleSystem;
        [SerializeField] private ParticleSystem m_GhostEmojiParticleSystem;
        [SerializeField] private ParticleSystem m_WindowEmojiParticleSystem;

        [SerializeField] private InputActionReference m_EnterInputRef;

        public void Start()
        {
            m_InteractionService.OnMessage += OnMessage;
            m_EnterInputRef.action.performed += ChangeScene;

        }

        private void ChangeScene(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene(kGameSceneIndex);
        }

        public void OnDisable()
        {
            m_InteractionService.OnMessage -= OnMessage;
            m_EnterInputRef.action.performed -= ChangeScene;
        }

        private void OnMessage(string message)
        {
            switch (message)
            {
                /*
                case "emoji-0": m_LightbulbEmojiParticleSystem?.Emit(1); m_InteractionService.Send(new string[] { UnityEngine.Random.Range(1, 5).ToString(), UnityEngine.Random.Range(1, 5).ToString(), UnityEngine.Random.Range(1, 5).ToString() }); break;
                case "emoji-1": m_HeartEmojiParticleSystem?.Emit(1); m_InteractionService.Send(new string[] { UnityEngine.Random.Range(1, 5).ToString(), UnityEngine.Random.Range(1, 5).ToString(), UnityEngine.Random.Range(1, 5).ToString() }); break;
                case "emoji-2": m_GhostEmojiParticleSystem?.Emit(1); m_InteractionService.Send(new string[] { UnityEngine.Random.Range(1, 5).ToString(), UnityEngine.Random.Range(1, 5).ToString(), UnityEngine.Random.Range(1, 5).ToString() }); break;
                case "emoji-3": m_WindowEmojiParticleSystem?.Emit(1); m_InteractionService.Send(new string[] { UnityEngine.Random.Range(1, 5).ToString(), UnityEngine.Random.Range(1, 5).ToString(), UnityEngine.Random.Range(1, 5).ToString() }); break;
                */
                
                case "emoji-0": m_LightbulbEmojiParticleSystem?.Emit(1); break;
                case "emoji-1": m_HeartEmojiParticleSystem?.Emit(1); break;
                case "emoji-2": m_GhostEmojiParticleSystem?.Emit(1); break;
                case "emoji-3": m_WindowEmojiParticleSystem?.Emit(1); break;
                
            }
        }
    }
}
#endif