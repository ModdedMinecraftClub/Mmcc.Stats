using Discord.Webhook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models.Settings;
using Mmcc.Stats.Infrastructure.Authentication;
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
            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            services.Configure<WebhookSettings>(Configuration.GetSection(nameof(WebhookSettings)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<WebhookSettings>>().Value);
            
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddScoped(provider =>
            {
                var url = provider.GetRequiredService<WebhookSettings>().WebhookUrl;
                return new DiscordWebhookClient(url);
            });
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDbTableService, DbTableService>();
            services.AddScoped<IPingService, PingService>();
            services.AddScoped<IServerService, ServerService>();
            services.AddScoped<ITpsService, TpsService>();
            services.AddScoped<IPlayerbaseService, PlayerbaseService>();
            services.AddScoped<IPollerService, PollerService>();
            services.AddScoped<IWebhookService, WebhookService>();
            services.AddScoped<ITpsProcessingService, TpsProcessingService>();
            
            services.AddAuthentication("ClientApp")
                .AddScheme<ClientAppAuthenticationOptions, ClientAppAuthenticationHandler>("ClientApp", null);
            services.AddAntiforgery(options => options.HeaderName = "X-Auth-Token");
            
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
