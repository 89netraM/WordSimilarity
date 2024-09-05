using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WordSimilarity;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets(Assembly.GetEntryAssembly()!);

builder.Services.AddOpenAIEmbeddingClient();
builder.Services.AddEmbeddingService();

builder.Services.AddOptions<GameConfig>().BindConfiguration("GameConfig");

var app = builder.Build();

app.MapGet(
    "/similarity",
    async (
        [FromServices] EmbeddingService service,
        [FromServices] IOptionsMonitor<GameConfig> gameConfig,
        [FromQuery] string word
    ) =>
    {
        var similarity = await service.GetSimilarityAsync(gameConfig.CurrentValue.SecretWord, word);
        return Results.Json(similarity);
    }
);

app.Run();
