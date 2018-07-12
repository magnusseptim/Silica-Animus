using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Runtime.Api;

namespace AbominableIntelligence.Model
{
    public class SentimentData
    {
        [Column(ordinal: "0", name: "Label")]
        public float sentiment;

        [Column(ordinal:"1")]
        public string sentimentText;
    }
}
