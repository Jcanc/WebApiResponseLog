using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiResponseLog.LogHelpers
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiLogService _apiLogService;
        private readonly IConfiguration _configration;

        public LogMiddleware(RequestDelegate next, IApiLogService apiLogService, IConfiguration configration)
        {
            _next = next;
            _apiLogService = apiLogService;
            _configration = configration;
        }

        public Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(() =>
            {
                var isLog = _configration.GetValue<bool>("ApiLog:IsEnable");
                if (isLog)
                {
                    watch.Stop();
                    _apiLogService.DataSave(context, watch.ElapsedMilliseconds);
                }
                return Task.CompletedTask;
            });
            
            return this._next(context);
        }
    }
}
