using System;
using System.Collections.Generic;
using Abominable_Intelligence.Model;
using Microsoft.AspNetCore.Mvc;
using Silica_Animus.Model;

namespace Silica_Animus.Builders
{
    public interface IPredictionResultBuilder
    {
        JsonResult BuildJsonPredictionResult(IEnumerable<SentimentPrediction> prediction, IEnumerable<ISentimentRequest> request);
        JsonResult BuildJsonPredictionResult(SentimentPrediction prediction, ISentimentRequest request);
        JsonResult BuildExceptionResult(Exception ex);
    }
}