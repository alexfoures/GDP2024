#if SECRET_VERSION
using MessagePipe;
using System;
using VContainer;
using VContainer.Unity;

namespace GDP2024
{
    public class WebsocketGameEventController : IGameEventController, IStartable, IDisposable
    {
        public bool Enabled { get ; set ; }

        [Inject] SpectatorInteractionService m_InteractionService;
        [Inject] IPublisher<GameEvent> m_GameEventPublisher;

        public void Start()
        {
            m_InteractionService.OnMessage += OnMessage;
        }

        public void Dispose()
        {
            m_InteractionService.OnMessage -= OnMessage;
        }

        private void OnMessage(string message)
        {
            switch (message)
            {
                case "0": m_GameEventPublisher.Publish(GameEvent.ONE); break;
                case "1": m_GameEventPublisher.Publish(GameEvent.TWO); break;
                case "2": m_GameEventPublisher.Publish(GameEvent.KEEP); break;
            }
        }
    }
}
#endif