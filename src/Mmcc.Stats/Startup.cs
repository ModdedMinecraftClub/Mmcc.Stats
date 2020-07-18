using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Models.Settings;
using Mmcc.Stats.Infrastructure.HostedServices;
using Mmcc.Stats.Infrastructure.Services;

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
            
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IPlayerbaseStatsService, PlayerbaseStatsService>();
            
            services.AddScoped<IPollerService, PollerService>();
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

            app.UseHttpsRedirection();

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
