using Microsoft.ML;
using System;

namespace AbominableIntelligence.Extension
{
    internal static class ModelExtension
    {
        public static (LearningPipeline, bool, string) ProcessNextStep(this (LearningPipeline, bool, string) state, Action<LearningPipeline> del)
        {
            (LearningPipeline, bool, string) result;
            try
            {
                del(state.Item1);
                result = (state.Item1,true, "OK");
            }
            catch (Exception ex)
            {
                result = (state.Item1, false, ex.Message);
            }

            return result;
        }

        public static (PredictionModel<DataModel, PredictedDataModel>,bool,string) ReturnTrained<DataModel, PredictedDataModel, Pipeline>
            (
                this (Pipeline,bool, string) state, 
                Func<Pipeline,PredictionModel<DataModel, PredictedDataModel>> del
            ) where DataModel : class
                                                                                                                                                                                                                                   where PredictedDataModel : class, new()
        {
            (PredictionModel<DataModel, PredictedDataModel>, bool, string) result;
            PredictionModel<DataModel, PredictedDataModel> model;
            try
            {
                model = del(state.Item1);
                result = (model, true, "OK");
            }
            catch (Exception ex)
            {
                model = null;
                result = (model, false, ex.Message);
            }

            return result;
        }
    }
}
