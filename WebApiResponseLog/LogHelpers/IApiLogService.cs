using Microsoft.AspNetCore.Http;

namespace WebApiResponseLog.LogHelpers
{
    public interface IApiLogService
    {
        void DataSave(HttpContext context, long responseTime);
    }
}
