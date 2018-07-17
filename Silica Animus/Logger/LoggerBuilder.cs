using Silica_Animus.Exceptions;
using System;

namespace Silica_Animus.Logger
{
    public class LoggerBuilder
    {
        public NLog.Logger BuildDefault()
        {
            try
            {
                return NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            }
            catch(Exception ex)
            {
                throw new LoggerException(ex.Message);
            }
        }

    }
}
