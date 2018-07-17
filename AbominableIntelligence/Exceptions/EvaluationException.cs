using Abominable_Intelligence.Enums;
using System;

namespace Abominable_Intelligence.Exceptions
{
    // TODO : Check if logger here will be better
    public class EvaluationException : Exception
    {
        public EvaluationException(string message) : base(message)
        {

        }

        public EvaluationException(string message, LearningStage stage) : base(message)
        {

        }
    }
}
