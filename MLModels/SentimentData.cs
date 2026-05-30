using Microsoft.ML.Data;

namespace TaskManagerApi.MLModels;

public class SentimentData
{
    [LoadColumn(0)]
    public string Text { get; set; } = string.Empty;

    [LoadColumn(1)]
    public bool Label { get; set; }
}