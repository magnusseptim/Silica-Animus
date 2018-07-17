using Abominable_Intelligence.Enums;
using Abominable_Intelligence.Exceptions;
using Abominable_Intelligence.Extension;
using Abominable_Intelligence.Model;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace Abominable_Intelligence.Prediction
{
    public class PEngine<DataModel,PredictedDataModel> : IPEngine<DataModel, PredictedDataModel> where DataModel : class, new() 
                                                                                                 where PredictedDataModel : class, new()
    {
        private string LearningDataPath { get; }
        private string EvaluateDataPath { get; }
        private ConfigData Config { get; set; }
        private LearningPipeline PiEngine { get; }
        private PredictionModel<DataModel, PredictedDataModel> Model { get; set; }

        public PEngine(ConfigData config = null)
        {
            this.Config = config;
            if (Config == null)
            {
                Config = BuildInConfigExtension<ConfigData>.ReadAsObject(Path.Combine(Environment.CurrentDirectory, "BuiltInConfig.json"));
            }

            this.LearningDataPath = Path.Combine(Environment.CurrentDirectory, Config.TrainingDataFolderName, Config.SADataSetaName);
            this.EvaluateDataPath = Path.Combine(Environment.CurrentDirectory, Config.TrainingDataFolderName, Config.SAEvaluateDataSet);

            PiEngine = new LearningPipeline();
        }

        public PredictedDataModel Predict(DataModel input)
        {
            if (Model == null)
            {
                throw new NullReferenceException("Model should be created first");
            }

            return Model.Predict(input);
        }
        public IEnumerable<PredictedDataModel> Predict(IEnumerable<DataModel> input)
        {
            if (Model == null)
            {
                throw new NullReferenceException("Model should be created first");
            }

            return Model.Predict(input);
        }


        public void Train(Logger logger)
        {
            try
            {
                Model = ProcessFirstStep((pipeline, dataPath) =>
                {
                    pipeline.Add(new TextLoader(this.LearningDataPath).CreateFrom<DataModel>());
                }, this.PiEngine, this.LearningDataPath, logger)
                .ProcessExceptionChecking()
                .ProcessNextStep((pipeline) =>
                {
                    pipeline.Add(new TextFeaturizer("Features", "sentimentText"));
                }, LearningStage.FeatureEngineering)
                .ProcessExceptionChecking()
                .ProcessNextStep((pipeline) =>
                {
                    pipeline.Add(new FastTreeBinaryClassifier()
                    {
                        NumLeaves = Config.TBC.NumLeaves,
                        NumTrees = Config.TBC.NumTrees,
                        MinDocumentsInLeafs = Config.TBC.MinDocumentsInLeafs
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
            catch (Exception)
            {
                throw new NullReferenceException("Logger needed");
            }
        }

        public BinaryClassificationMetrics Evaluate()
        {
            try
            {
                return new BinaryClassificationEvaluator().Evaluate(this.Model, new TextLoader(EvaluateDataPath)
                                                          .CreateFrom<SentimentData>());
            }
            catch (Exception ex)
            {
                throw new EvaluationException(ex.Message);
            }
        }

        private (LearningPipeline,bool, string,LearningStage,Logger) ProcessFirstStep
        (
            Action<LearningPipeline,string> data, 
            LearningPipeline pipeline, 
            string dataPath, 
            Logger logger
        )
        {
            (LearningPipeline, bool, string, LearningStage,Logger) result;
            try
            {
                data(pipeline,dataPath);
                result = (pipeline,true, "OK",LearningStage.IngestData,logger);
            }
            catch (Exception ex)
            {
                result = (pipeline,false, ex.Message,LearningStage.IngestData,logger);
            }

            return result;
        }
    }
}
