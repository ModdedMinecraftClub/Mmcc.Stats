using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models.Settings;
using Mmcc.Stats.Infrastructure.HostedServices;
using Mmcc.Stats.Infrastructure.Services;
using Mmcc.Stats.Infrastructure.Services.DataAccess;

namespace Mmcc.Stats
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
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            
            services.AddScoped<IDbTablesService, DbTablesService>();
            services.AddScoped<IPingsService, PingsService>();
            services.AddScoped<IServersService, ServersService>();
            services.AddScoped<ITpsService, TpsService>();
            services.AddScoped<IServersPlayerbaseService, ServersPlayerbaseService>();
            services.AddScoped<IPollerService, PollerService>();

            services.AddHostedService<StartupChecksHostedService>();
            services.AddHostedService<PollerTimedHostedService>();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
        
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
