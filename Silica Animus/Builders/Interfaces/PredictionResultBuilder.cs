using Abominable_Intelligence.Model;
using Microsoft.AspNetCore.Mvc;
using Silica_Animus.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silica_Animus.Builders
{
    public class PredictionResultBuilder : IPredictionResultBuilder
    {
        public JsonResult BuildJsonPredictionResult(SentimentPrediction prediction, ISentimentRequest request)
        {
            return new JsonResult(BuildPredictionResult(prediction, request));
        }

        public JsonResult BuildJsonPredictionResult(IEnumerable<SentimentPrediction> prediction, IEnumerable<ISentimentRequest> request)
        {
            return new JsonResult(BuildPredictionResult(prediction, request));
        }

        public JsonResult BuildExceptionResult(Exception ex)
        {
            return new JsonResult($"Exception during process : {ex.Message}");
        }

        private (string text, bool sentiment) BuildPredictionResult(SentimentPrediction prediction, ISentimentRequest request)
        {
            return (text: request.SentimentText, sentiment: prediction.Sentiment);
        }

        private IEnumerable<(string text, bool sentiment)> BuildPredictionResult(IEnumerable<SentimentPrediction> prediction, IEnumerable<ISentimentRequest> request)
        {
            return request.Zip(prediction.Select(x => x.sentiment).ToList(), (r, p) => (r.SentimentText, p));
        }
    }
}
