using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;
using WebApiResponseLog.LogHelpers;

namespace WebApiResponseLog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IApiLogService, ApiLogService>();
            services.AddSingleton(new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = "server=.;uid=sa;pwd=sasa;database=apilogdemo;MultipleActiveResultSets=true",//����, ���ݿ������ַ���
                DbType = DbType.SqlServer,         //����, ���ݿ�����
                IsAutoCloseConnection = true,       //Ĭ��false, ʱ��֪���ر����ݿ�����, ����Ϊtrue����ʹ��using����Close����
                InitKeyType = InitKeyType.SystemTable    //Ĭ��SystemTable, �ֶ���Ϣ��ȡ, �磺�������ǲ����������ǲ��Ǳ�ʶ�еȵ���Ϣ
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseLogMiddleware();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
