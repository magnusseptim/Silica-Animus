using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Silica_Animus.Logger;

namespace Silica_Animus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLog.ILogger logger = new LoggerBuilder().BuildDefault();
            try
            {
                BuildWebHost(args).Run();
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw ex;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logger =>
                {
                    logger.ClearProviders();
                    logger.SetMinimumLevel(LogLevel.Information);
                })
                .UseNLog()
                .Build();
    }
}
