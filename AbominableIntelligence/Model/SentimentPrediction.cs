using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbominableIntelligence.Model
{
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool sentiment;
    }
}
