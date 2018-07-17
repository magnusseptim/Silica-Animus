using Microsoft.ML.Runtime.Api;

namespace Abominable_Intelligence.Model
{
    public class SentimentPrediction : ISentimentPrediction
    {
        public bool Sentiment
        {
            get
            {
                return sentiment;
            }
            set
            {
                sentiment = value;
            }
        }
        [ColumnName("PredictedLabel")]
        public bool sentiment;
    }
}
