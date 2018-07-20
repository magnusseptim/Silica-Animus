using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silica_Animus.Model
{
    public class SentimentRequest : ISentimentRequest
    {
        public string SentimentText { get; set; }
    }
}
