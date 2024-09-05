using System.ComponentModel.DataAnnotations;

namespace WordSimilarity;

public class GameConfig
{
    [Required]
    public required string SecretWord { get; init; }
}
