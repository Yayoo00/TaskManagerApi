using Microsoft.ML;
using TaskManagerApi.MLModels;

namespace TaskManagerApi.Services;

public class SentimientoService
{
    private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

    public SentimientoService()
    {
        var mlContext = new MLContext();

        var data = mlContext.Data.LoadFromTextFile<SentimentData>(
            "Dataset/sentimientos.csv",
            hasHeader: true,
            separatorChar: ',');

        var pipeline = mlContext.Transforms.Text
            .FeaturizeText("Features", nameof(SentimentData.Text))
            .Append(
                mlContext.BinaryClassification.Trainers
                .SdcaLogisticRegression());

        var model = pipeline.Fit(data);

        _predictionEngine =
            mlContext.Model.CreatePredictionEngine
            <SentimentData, SentimentPrediction>(model);
    }

    public bool Predict(string texto)
    {
        var prediction =
            _predictionEngine.Predict(
                new SentimentData
                {
                    Text = texto
                });

        return prediction.Prediction;
    }
}