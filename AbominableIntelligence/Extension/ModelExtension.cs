using Abominable_Intelligence.Enums;
using Abominable_Intelligence.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using NLog;
using System;

namespace Abominable_Intelligence.Extension
{
    internal static class ModelExtension
    {
        public static (LearningPipeline, bool, string, LearningStage,Microsoft.Extensions.Logging.ILogger) ProcessNextStep(this (LearningPipeline, bool, string,LearningStage,Microsoft.Extensions.Logging.ILogger) state, Action<LearningPipeline> del, LearningStage stage)
        {
            (LearningPipeline, bool, string,LearningStage,Microsoft.Extensions.Logging.ILogger) result;
            try
            {
                del(state.Item1);
                result = (state.Item1,true, "OK",stage, state.Item5);
                state.Item5.LogInformation($"Stage {state.Item4.ToString()} processed correct");
            }
            catch (Exception ex)
            {
                result = (state.Item1, false, ex.Message, stage,state.Item5);
            }

            return result;
        }

        public static (LearningPipeline, bool, string,LearningStage,Microsoft.Extensions.Logging.ILogger) ProcessExceptionChecking(this (LearningPipeline, bool, string,LearningStage,Microsoft.Extensions.Logging.ILogger) state)
        {
            if (!state.Item2)
            {
                state.Item5.LogError($"Error {state.Item3} occurred on stage {state.Item4.ToString()}");
                throw new LearningException(state.Item3);
            }
            return state;
        }

        public static (PredictionModel<DataModel, PredictedDataModel>, bool, string, LearningStage,Microsoft.Extensions.Logging.ILogger) ProcessExceptionChecking<DataModel, PredictedDataModel>
        (
            this (PredictionModel<DataModel, PredictedDataModel>, bool, string, LearningStage,Microsoft.Extensions.Logging.ILogger) state
        )
        where DataModel : class
        where PredictedDataModel : class, new()
        {
            if (!state.Item2)
            {
                state.Item5.LogError($"Error {state.Item3} occurred on stage {state.Item4.ToString()}");
                throw new LearningException(state.Item3);
            }
            return state;
        }

        public static (PredictionModel<DataModel, PredictedDataModel>,bool,string,LearningStage,Microsoft.Extensions.Logging.ILogger) ReturnTrained<DataModel, PredictedDataModel, Pipeline>
        (
            this (Pipeline,bool, string,LearningStage,Microsoft.Extensions.Logging.ILogger) state, 
            Func<PredictionModel<DataModel, PredictedDataModel>> del
        ) where DataModel : class
          where PredictedDataModel : class, new()
        {
            (PredictionModel<DataModel, PredictedDataModel>, bool, string, LearningStage, Microsoft.Extensions.Logging.ILogger) result;
            PredictionModel<DataModel, PredictedDataModel> model;
            try
            {
                model = del();
                result = (model, true, "OK",LearningStage.Train, state.Item5);
            }
            catch (Exception ex)
            {
                model = null;
                result = (model, false, ex.Message, LearningStage.Train, state.Item5);
                state.Item5.LogInformation($"Stage {state.Item4.ToString()} processed correct");
            }

            return result;
        }
    }
}
