using System;
using System.Collections.Generic;
using System.Text;

namespace Abominable_Intelligence.Exceptions
{
    // TODO : Check if logger here will be better
    [Serializable]
    public class LearningException : Exception
    {
        public LearningException(string message) : base(message)
        {

        }
    }
}
