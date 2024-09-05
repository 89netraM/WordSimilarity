using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;

namespace WordSimilarity;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddOpenAIEmbeddingClient(this IServiceCollection services)
    {
        services
            .AddOptions<OpenAIConfig>()
            .BindConfiguration("OpenAI")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<OpenAIConfig>>().Value;
            return new EmbeddingClient(config.EmbeddingModel, config.ApiKey);
        });
        return services;
    }

    public static IServiceCollection AddEmbeddingService(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<EmbeddingService>();
        return services;
    }
}
