using Abominable_Intelligence.Exceptions;
using Abominable_Intelligence.Model;
using Abominable_Intelligence.Prediction;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using Silica_Animus.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SilicaAnimusTests.Prediction
{
    public class PEngineTests : IDisposable
    {
        private MockRepository mockRepository;

        public PEngineTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose)
            {
                DefaultValue = DefaultValue.Mock
            };
        }

        public void Dispose()
        {
            this.mockRepository.Verify();
        }

        private PEngine<SentimentData,SentimentPrediction> CreatePEngine()
        {
            return new PEngine<SentimentData, SentimentPrediction>();
        }

        [Fact]
        public void Predict_NullModel_ThrowNullException()
        {
            //Arrange
            IEnumerable<SentimentData> sentiments = new List<SentimentData>
            {
                new SentimentData
                {
                    sentimentText = "Please refrain from adding nonsense to Wikipedia."
                },
                new SentimentData
                {
                    sentimentText = "He is the best, and the article should say that."
                }
            };
            var model = CreatePEngine();
            //Act
            //Assert
            model.Invoking(x => x.Predict(sentiments))
                 .Should()
                 .Throw<NullReferenceException>()
                 .WithMessage("Model should be created first");
        }

        [Fact]
        public void Evaluation_NullModel_EvaluationException()
        {
            //Arrange
            var model = CreatePEngine();
            //Act
            //Assert
            model.Invoking(x => x.Evaluate())
                 .Should()
                 .Throw<EvaluationException>();
        }



        [Fact]
        public void Train_PredictSentiment_Correct()
        {
            // Arrange
            IEnumerable<SentimentData> sentiments = new List<SentimentData>
            {
                new SentimentData
                {
                    sentimentText = "Please refrain from adding nonsense to Wikipedia."
                },
                new SentimentData
                {
                    sentimentText = "He is the best, and the article should say that."
                }
            };
            IEnumerable<SentimentPrediction> expected = new List<SentimentPrediction>
            {
                new SentimentPrediction
                {
                    sentiment = false
                },
                new SentimentPrediction
                {
                    sentiment = true
                }
            };
           
            var unitUnderTest = CreatePEngine();

            Mock<Microsoft.Extensions.Logging.ILogger> fakeLogger = new Mock<Microsoft.Extensions.Logging.ILogger>();
            // Act
            unitUnderTest.Train(fakeLogger.Object);

            IEnumerable<SentimentPrediction> predictions = unitUnderTest.Predict(sentiments);

            IEnumerable<(string text,bool sentiment)> sentimentsAndPredictions = sentiments.Zip(predictions.Select(x=>x.sentiment).ToList(), (sentiment, prediction) => (sentiment.sentimentText, prediction));
            IEnumerable<(string text, bool sentiment)> sentimentsAndExpected = sentiments.Zip(expected, (sentiment, exp) => (sentiment.sentimentText, exp.sentiment));

            // Assert
            sentimentsAndPredictions.Should()
                                    .NotBeEmpty()
                                    .And.HaveCount(2)
                                    .And.ContainInOrder(sentimentsAndExpected);
        }

        [Fact]
        public void Evaluate_TrainingLevelPerecent_MoreThan_65_93_75()
        {
            // Arrange
            var unitUnderTest = CreatePEngine();
            Mock<Microsoft.Extensions.Logging.ILogger> fakeLogger = new Mock<Microsoft.Extensions.Logging.ILogger>();
            // Act
            unitUnderTest.Train(fakeLogger.Object);
            var evaluation = unitUnderTest.Evaluate();

            // Assert
            evaluation.Accuracy.Should().BeGreaterThan(.65);
            evaluation.Auc.Should().BeGreaterThan(.93);
            evaluation.F1Score.Should().BeGreaterThan(.74);
        }
    }
}
