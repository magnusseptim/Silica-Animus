using Abominable_Intelligence.Enums;
using Abominable_Intelligence.Exceptions;
using Microsoft.ML;
using System;

namespace Abominable_Intelligence.Extension
{
    internal static class ModelExtension
    {
        public static (LearningPipeline, bool, string, LearningStage) ProcessNextStep(this (LearningPipeline, bool, string,LearningStage) state, Action<LearningPipeline> del, LearningStage stage)
        {
            (LearningPipeline, bool, string,LearningStage) result;
            try
            {
                del(state.Item1);
                result = (state.Item1,true, "OK",stage);
            }
            catch (Exception ex)
            {
                result = (state.Item1, false, ex.Message,stage);
            }

            return result;
        }
        // To improove by using Logger
        public static (LearningPipeline, bool, string,LearningStage) ProcessExceptionChecking(this (LearningPipeline, bool, string,LearningStage) state)
        {
            if (!state.Item2)
            {
                //Logger here
                throw new LearningException(state.Item3);
            }
            return state;
        }

        // To improove by using Logger
        public static (PredictionModel<DataModel, PredictedDataModel>, bool, string, LearningStage) ProcessExceptionChecking<DataModel, PredictedDataModel>
        (
            this (PredictionModel<DataModel, PredictedDataModel>, bool, string, LearningStage) state
        )
        where DataModel : class
        where PredictedDataModel : class, new()
        {
            if (!state.Item2)
            {
                //Logger here
                throw new LearningException(state.Item3);
            }
            return state;
        }

        // TODO : Logger
        public static (PredictionModel<DataModel, PredictedDataModel>,bool,string,LearningStage) ReturnTrained<DataModel, PredictedDataModel, Pipeline>
        (
            this (Pipeline,bool, string,LearningStage) state, 
            Func<PredictionModel<DataModel, PredictedDataModel>> del
        ) where DataModel : class
          where PredictedDataModel : class, new()
        {
            (PredictionModel<DataModel, PredictedDataModel>, bool, string,LearningStage) result;
            PredictionModel<DataModel, PredictedDataModel> model;
            try
            {
                model = del();
                result = (model, true, "OK",LearningStage.Train);
            }
            catch (Exception ex)
            {
                model = null;
                result = (model, false, ex.Message, LearningStage.Train);
            }

            return result;
        }
    }
}
