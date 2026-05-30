using Microsoft.ML.Data;

namespace TaskManagerApi.MLModels;

public class SentimentPrediction
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }
}