using System.ComponentModel.DataAnnotations;

namespace WordSimilarity;

public class OpenAIConfig
{
    [Required]
    public required string ApiKey { get; init; }

    [Required]
    public required string EmbeddingModel { get; init; }
}
