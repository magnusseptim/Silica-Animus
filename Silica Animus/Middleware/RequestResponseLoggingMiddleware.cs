using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Silica_Animus.Extension;
using Silica_Animus.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Silica_Animus.Middleware
{
    public class RequestResponseLoggingMiddleware 
    {
        private readonly ILogger<RequestResponseLoggingMiddleware> logger;
        private readonly RequestDelegate next;

        public RequestResponseLoggingMiddleware(ILogger<RequestResponseLoggingMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = await FormatRequest(context.Request);
            LogRequest(request);
            var originalBody = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                LogResponse(context);

                await responseBody.CopyToAsync(originalBody);
            }
        }

        private async Task<(string scheme, string host, string path, string queryString, string bodyAsText)> FormatRequest(HttpRequest request)
        {
            var returned = (scheme: request.Scheme,host: request.Host.ToString(),path:  request.Path,queryString: "",bodyAsText: "");
            try
            {
                var body = request.Body;
                request.EnableRewind();
                returned.queryString = request.QueryString.ToString();
                returned.bodyAsText = await new StreamReader(request.Body).ReadToEndAsync(); 
                request.Body = body;
            }
            catch (Exception ex)
            {
                returned.bodyAsText = ex.Message;
            }
            return returned;
        }

        private async Task<(string statusCode, string text)> FormatResponse(HttpResponse response)
        {
            var returned = (statusCode: "", text: "");
            try
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                string text = await new StreamReader(response.Body).ReadToEndAsync();
                response.Body.Seek(0, SeekOrigin.Begin);
                returned.text = text;
                returned.statusCode = response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                returned.statusCode = 500.ToString();
                returned.text = ex.Message;
            }

            return returned;
        }

        private void LogRequest((string scheme, string host, string path, string queryString, string bodyAsText) request)
        {
            var checkObject = JsonConvert.DeserializeObject(request.bodyAsText);
            if (!checkObject.IsType(typeof(LogIn), typeof(Register)).foundInList)
            {
                logger.LogInformation($"Scheme: {request.scheme}, Host: {request.host}, Path: {request.path}, QueryString: {request.queryString}, Body: {request.bodyAsText}");
            }
        }

        private async void LogResponse(HttpContext context)
        {
            if (context.Response.Body.GetType() == typeof(JsonResult))
            {
                var response = await FormatResponse(context.Response);
                logger.LogInformation($"StatusCode : {response.statusCode}, Data : {response.text}");
            }
        }
    }
}
