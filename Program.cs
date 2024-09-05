using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WordSimilarity;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets(Assembly.GetEntryAssembly()!);

builder.Services.AddOpenAIEmbeddingClient();
builder.Services.AddEmbeddingService();

var app = builder.Build();

app.MapGet(
    "/similarity",
    async (
        [FromServices] EmbeddingService service,
        [FromQuery] string word1,
        [FromQuery] string word2
    ) =>
    {
        var similarity = await service.GetSimilarityAsync(word1, word2);
        return Results.Json(similarity);
    }
);

app.Run();
