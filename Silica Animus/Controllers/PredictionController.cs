using Abominable_Intelligence.Model;
using Abominable_Intelligence.Prediction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Silica_Animus.Builders;
using Silica_Animus.Extension;
using Silica_Animus.Model;
using System;
using System.Collections.Generic;

namespace Silica_Animus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PredictionController : Controller
    {
        private readonly ILogger logger;
        private readonly IPEngine<SentimentData, SentimentPrediction> pEngine;
        private readonly IPredictionResultBuilder predictionResultBuilder;
        public PredictionController(ILogger logger, IPEngine<SentimentData, SentimentPrediction> pEngine, IPredictionResultBuilder predictionResultBuilder)
        {
            this.logger = logger;
            this.pEngine = pEngine;
            this.predictionResultBuilder = predictionResultBuilder;
            pEngine.Train(logger);
        }

        [HttpPost,Authorize]
        public IActionResult CheckIfSentimentText([FromBody] ISentimentRequest request)
        {
            IActionResult result;
            try
            {
                result = predictionResultBuilder.BuildJsonPredictionResult(pEngine.Predict(request.ToSentimentData()),request);
            }
            catch (Exception ex)
            {
                result = predictionResultBuilder.BuildExceptionResult(ex);
            }

            return result;
        }

        [HttpPost,Authorize]
        public IActionResult CheckIfSentimentTexts([FromBody] IEnumerable<ISentimentRequest> request)
        {
            IActionResult result;
            try
            {
                result = predictionResultBuilder.BuildJsonPredictionResult(pEngine.Predict(request.ToSentimentData()), request);
            }
            catch (Exception ex)
            {
                result = predictionResultBuilder.BuildExceptionResult(ex);
            }

            return result;
        }
    }
}
