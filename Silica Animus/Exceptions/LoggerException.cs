using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silica_Animus.Exceptions
{
    public class LoggerException : Exception
    {
        public LoggerException(string message) : base(message)
        {

        }
    }
}
