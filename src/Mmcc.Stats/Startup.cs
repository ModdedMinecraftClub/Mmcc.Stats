using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
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
using Mmcc.Stats.Infrastructure.Commands;
using Mmcc.Stats.Infrastructure.HostedServices;
using Mmcc.Stats.Infrastructure.Services;
using NSwag;
using NSwag.Generation.Processors.Security;
using TraceLd.DiscordWebhook;
using TraceLd.DiscordWebhook.Models;
using ZymLabs.NSwag.FluentValidation;
using ZymLabs.NSwag.FluentValidation.AspNetCore;

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
            
            services.AddHttpContextAccessor();
            services.AddHttpClient<IWebhookService, WebhookService>();

            services.AddDbContext<PollerContext>((provider, builder) =>
                {
                    var dbConfig = provider.GetRequiredService<DatabaseSettings>();
                    var connString =
                        $"server={dbConfig.Server};database={dbConfig.DatabaseName};uid={dbConfig.Username};pwd={dbConfig.Password}";
                    var serverVersion = ServerVersion.FromString("10.4.11-mariadb");
                    
                    builder.UseMySql(connString, serverVersion);
                });

            services.AddMediatR(typeof(Startup), typeof(NotifyStaffAboutTps));

            services.AddSingleton<FluentValidationSchemaProcessor>();
            services.AddScoped<IPollerService, PollerService>();
            
            services.AddHostedService<PollerTimedHostedService>();

            services.AddAuthentication("ClientApp")
                .AddScheme<ClientAppAuthenticationOptions, ClientAppAuthenticationHandler>("ClientApp", null);
            services.AddAntiforgery(options => options.HeaderName = "X-Auth-Token");
            
            services.AddControllers()
                .AddFluentValidation(configuration =>
                {
                    configuration.RegisterValidatorsFromAssemblyContaining<Startup>();
                    configuration.ImplicitlyValidateChildProperties = true;
                    configuration.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
                });

            services.AddOpenApiDocument((settings, provider) =>
            {
                settings.DocumentName = "openapi";
                
                // add authorization info;
                settings.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("ClientApp",
                        new OpenApiSecurityScheme
                        {
                            Type = OpenApiSecuritySchemeType.ApiKey,
                            Description = "Authentication used for client apps, such as Mmcc.Stats.TpsMonitor",
                            Name = "X-Auth-Token",
                            In = OpenApiSecurityApiKeyLocation.Header
                        }));
                settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("ClientApp"));

                // add fluent validation;
                var fluentValidationSchemaProcessor = provider.GetService<FluentValidationSchemaProcessor>();
                settings.SchemaProcessors.Add(fluentValidationSchemaProcessor);
                
                // add basic info;
                settings.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "MMCC Stats API";
                    document.Info.Description = "API for MMCC Statistics.";
                    document.Info.License = new OpenApiLicense
                    {
                        Name = "GNU Affero General Public License v3.0",
                        Url = "https://github.com/ModdedMinecraftClub/Mmcc.Stats/blob/master/LICENSE"
                    };
                };
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

            app.UseOpenApi(settings =>
            {
                settings.DocumentName = "openapi";
                settings.Path = "/openapi/v1/openapi.json";
            });
            app.UseReDoc(settings =>
            {
                settings.Path = "/docs";
                settings.DocumentPath = "/openapi/v1/openapi.json";
            });
            
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
