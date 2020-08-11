using System.Net.Mime;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models.Settings;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Infrastructure.Authentication;
using Mmcc.Stats.Infrastructure.Authentication.ClientAppAuthentication;
using Mmcc.Stats.Infrastructure.HostedServices;
using Mmcc.Stats.Infrastructure.Services;
using TraceLd.DiscordWebhook;
using TraceLd.DiscordWebhook.Models;

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
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            
            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            services.Configure<WebhookSettings>(Configuration.GetSection(nameof(WebhookSettings)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<WebhookSettings>>().Value);
            services.Configure<TpsPingSettings>(Configuration.GetSection(nameof(TpsPingSettings)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<TpsPingSettings>>().Value);

            services.AddHttpClient<IWebhookService, WebhookService>();

            services.AddDbContext<PollerContext>((provider, builder) =>
                {
                    var dbConfig = provider.GetRequiredService<DatabaseSettings>();
                    builder.UseMySql(
                        $"server={dbConfig.Server};database={dbConfig.DatabaseName};uid={dbConfig.Username};pwd={dbConfig.Password}");
                });

            services.AddMediatR(typeof(Startup));
            services.AddScoped<IPollerService, PollerService>();
            
            services.AddAuthentication("ClientApp")
                .AddScheme<ClientAppAuthenticationOptions, ClientAppAuthenticationHandler>("ClientApp", null);
            services.AddAntiforgery(options => options.HeaderName = "X-Auth-Token");
            
            services.AddHostedService<PollerTimedHostedService>();

            services.AddControllers()
                .AddFluentValidation(configuration =>
                {
                    configuration.RegisterValidatorsFromAssemblyContaining<Startup>();
                    configuration.ImplicitlyValidateChildProperties = true;
                });
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
