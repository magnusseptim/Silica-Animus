using Microsoft.ML;
using Microsoft.ML.Models;
using NLog;
using System.Collections.Generic;

namespace Abominable_Intelligence.Prediction
{
    public interface IPEngine<DataModel, PredictedDataModel>
        where DataModel : class, new()
        where PredictedDataModel : class, new()
    {
        PredictedDataModel Predict(DataModel input);
        IEnumerable<PredictedDataModel> Predict(IEnumerable<DataModel> input);
        BinaryClassificationMetrics Evaluate();
        void Train(Logger logger);
    }
}