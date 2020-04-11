using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.IO;
using WebApiResponseLog.Entities;

namespace WebApiResponseLog.LogHelpers
{
    public class ApiLogService : IApiLogService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlSugarClient _dbContext;

        public ApiLogService(IConfiguration configuration, SqlSugarClient dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public void DataSave(HttpContext context, long responseTime)
        {
            var isLog = _configuration.GetValue<bool>("ApiLog:IsEnable");
            if (isLog)
            {
                var requestMethod = context.Request.Method;
                var requestURL = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
                //var accessToken = context.GetTokenAsync("access_token").Result;
                var accessToken = string.Empty;
                var requestBody = string.Empty;
                if (requestMethod == "POST")
                {
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    var _reader = new StreamReader(context.Request.Body);
                    requestBody = _reader.ReadToEnd();
                }
                _dbContext.Insertable(new ApiLog
                {
                    AccessToken = accessToken,
                    AccessTime = DateTime.Now,
                    AccessAction = requestMethod,
                    AccessApiUrl = requestURL,
                    QueryString = context.Request.QueryString.ToString(),
                    Body = requestBody,
                    HttpStatus = context.Response.StatusCode,
                    ClientIP = context.Connection.RemoteIpAddress.ToString(),
                    ResponseTime = responseTime
                }).ExecuteCommand();
            }
        }
    }
}
