using System;
using System.Numerics.Tensors;
using OpenAI.Embeddings;

Console.WriteLine("\x1b[2J");

var client = new EmbeddingClient(
    "text-embedding-3-large",
    Environment.GetEnvironmentVariable("OPENAI_API_KEY")
        ?? throw new Exception("Missing OPENAI_API_KEY environment variable")
);

var secretWord = args[0].Trim().ToLowerInvariant();
var secretWordVector = client.GenerateEmbedding(secretWord).Value.Vector;

while (true)
{
    Console.Write("Enter a word to guess: ");
    var guessWord = Console.ReadLine()!.Trim().ToLowerInvariant();
    if (guessWord == secretWord)
    {
        Console.WriteLine("Correct!");
        break;
    }
    var guessWordVector = client.GenerateEmbedding(guessWord).Value.Vector;
    var similarity = TensorPrimitives.CosineSimilarity(secretWordVector.Span, guessWordVector.Span);
    Console.WriteLine($"Similarity: {similarity}");
}
