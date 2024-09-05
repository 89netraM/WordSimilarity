using System.Numerics.Tensors;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OpenAI.Embeddings;

namespace WordSimilarity;

public class EmbeddingService(
    ILogger<EmbeddingService> logger,
    EmbeddingClient embeddingClient,
    IMemoryCache cache
)
{
    public async Task<float> GetSimilarityAsync(string word1, string word2)
    {
        var vector1 = await GetEmbedding(word1);
        var vector2 = await GetEmbedding(word2);
        var similarity = TensorPrimitives.CosineSimilarity(vector1, vector2);
        return similarity;
    }

    private ValueTask<float[]> GetEmbedding(string word)
    {
        if (cache.TryGetValue<float[]>(word, out var vector) && vector is not null)
        {
            return ValueTask.FromResult(vector);
        }

        return new(GetEmbeddingAsync(word));
    }

    private async Task<float[]> GetEmbeddingAsync(string word)
    {
        var embeddingResponse = await embeddingClient.GenerateEmbeddingAsync(word);
        var vector = embeddingResponse.Value.Vector.ToArray();
        logger.LogDebug("Caching embedding for {Word}", word);
        cache.Set(word, vector);
        return vector;
    }
}
