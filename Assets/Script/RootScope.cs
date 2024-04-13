using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GDP2024
{
    public class RootScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));

            builder.UseEntryPoints((entryPoints) =>
            {
#if SECRET_VERSION
                entryPoints.Add<SpectatorInteractionService>().AsSelf();
#endif
            });

        }
    }
}
