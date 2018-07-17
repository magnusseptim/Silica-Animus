using Abominable_Intelligence.Enums;
using System;

namespace Abominable_Intelligence.Exceptions
{
    // TODO : Check if logger here will be better
    [Serializable]
    public class LearningException : Exception
    {
        public LearningException(string message) : base(message)
        {

        }

        public LearningException(string message, LearningStage stage) : base(message)
        {

        }
    }
}
