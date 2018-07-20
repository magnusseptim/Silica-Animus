using Abominable_Intelligence.Model;
using Silica_Animus.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silica_Animus.Extension
{
    public static class SentimentConversion
    {
        public static SentimentData ToSentimentData(this ISentimentRequest request)
        {
            return (SentimentData)Activator.CreateInstance(typeof(SentimentData), request.SentimentText);
        }

        public static IEnumerable<SentimentData> ToSentimentData(this IEnumerable<ISentimentRequest> request)
        {
            return request.Select(x => x.ToSentimentData()).ToList();
        }
    }
}
