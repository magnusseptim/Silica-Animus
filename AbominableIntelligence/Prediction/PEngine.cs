using Microsoft.ML;
using System;

namespace AbominableIntelligence.Prediction
{
    public class PEngine<DataModel,PredictedDataModel> where DataModel : class, new() 
                                                       where PredictedDataModel : class, new()
    {
        private string LearningDataPath { get; }
        private string EvaluateDataPath { get; }
        private LearningPipeline PiEngine { get; }

        public PEngine(string datasetPath = "", string evaluateDataPath = "")
        {
            this.LearningDataPath = datasetPath;
            this.EvaluateDataPath = evaluateDataPath;
            PiEngine = new LearningPipeline();
        }

        public PredictionModel<DataModel, PredictedDataModel> Train()
        {
            throw new NotImplementedException();
        }

        private (LearningPipeline,bool, string) ProcessFirstStep(Action<LearningPipeline> data, LearningPipeline pipeline)
        {
            (LearningPipeline, bool, string) result;
            try
            {
                data(pipeline);
                result = (pipeline,true, "OK");
            }
            catch (Exception ex)
            {
                result = (pipeline,false, ex.Message);
            }

            return result;
        }
    }
}
