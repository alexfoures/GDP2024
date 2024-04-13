using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GDP2024
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private GameEventProcessor m_GameEventProcessor;

        protected override void Configure(IContainerBuilder builder)
        {
            // Configure Message Pipes
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<GameEvent>(options);

            builder.RegisterComponent(m_GameEventProcessor);

            builder.UseEntryPoints((entryPoints) =>
            {
#if SECRET_VERSION
                entryPoints.Add<WebsocketGameEventController>();
#endif
            });
        }
    }
}