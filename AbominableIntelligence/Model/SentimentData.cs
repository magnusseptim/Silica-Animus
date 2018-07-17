using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Runtime.Api;

namespace Abominable_Intelligence.Model
{
    public class SentimentData : ISentimentData
    {
        public float Sentiment
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

        [Column(ordinal: "0", name: "Label")]
        public float sentiment;

        public string SentimentText
        {
            get
            {
                return sentimentText;
            }
            set
            {
                sentimentText = value;
            }
        }

        [Column(ordinal:"1")]
        public string sentimentText;
    }
}
