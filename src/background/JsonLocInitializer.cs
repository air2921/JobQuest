using JsonLocalizer;
using Microsoft.Extensions.Hosting;

namespace background;

internal class JsonLocInitializer(ILocalizer localizer, LocalizerOptions localizerOptions) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var lang in localizerOptions.SupportedLanguages)
            localizer.Initialize(lang);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
