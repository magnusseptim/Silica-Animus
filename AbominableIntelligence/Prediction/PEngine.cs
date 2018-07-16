using Abominable_Intelligence.Enums;
using Abominable_Intelligence.Extension;
using Abominable_Intelligence.Model;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.IO;

namespace Abominable_Intelligence.Prediction
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

            if (string.IsNullOrEmpty(this.LearningDataPath) || string.IsNullOrEmpty(this.EvaluateDataPath))
            {
                ConfigData config = BuildInConfigExtension<ConfigData>.ReadAsObject(Path.Combine(Environment.CurrentDirectory, "BuiltInConfig.json"));
                this.LearningDataPath = Path.Combine(Environment.CurrentDirectory, config.TrainingDataFolderName, config.SADataSetaName);
                this.EvaluateDataPath = Path.Combine(Environment.CurrentDirectory, config.TrainingDataFolderName, config.SAEvaluateDataSet);
            }

            PiEngine = new LearningPipeline();
        }

        public PredictionModel<DataModel, PredictedDataModel> Train()
        {
            return ProcessFirstStep((pipeline, dataPath) =>
            {
                pipeline.Add(new TextLoader(this.LearningDataPath).CreateFrom<DataModel>());
            }, this.PiEngine, this.LearningDataPath)
            .ProcessExceptionChecking()
            .ProcessNextStep((pipeline) =>
            {
                pipeline.Add(new TextFeaturizer("Features", "SentimentText"));
            }, LearningStage.FeatureEngineering)
            .ProcessExceptionChecking()
            .ProcessNextStep((pipeline) =>
            {
                // TODO: Remove hard coded values
                pipeline.Add(new FastTreeBinaryClassifier()
                {
                    NumLeaves = 5,
                    NumTrees = 5,
                    MinDocumentsInLeafs = 2
                });
            }, LearningStage.LearningAlg) 
            .ProcessExceptionChecking()
            .ReturnTrained(() =>
            {
                return PiEngine.Train<DataModel, PredictedDataModel>();
            })
            .ProcessExceptionChecking()
            .Item1;
            
        }

        private (LearningPipeline,bool, string,LearningStage) ProcessFirstStep(Action<LearningPipeline,string> data, LearningPipeline pipeline, string dataPath)
        {
            (LearningPipeline, bool, string, LearningStage) result;
            try
            {
                data(pipeline,dataPath);
                result = (pipeline,true, "OK",LearningStage.IngestData);
            }
            catch (Exception ex)
            {
                result = (pipeline,false, ex.Message,LearningStage.IngestData);
            }

            return result;
        }
    }
}
