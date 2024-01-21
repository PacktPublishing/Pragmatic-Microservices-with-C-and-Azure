using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Configuration;

#if PROMETHEUS
#else
using Azure.Core.Diagnostics;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;

using Microsoft.Extensions.Configuration.AzureAppConfiguration;
#endif

namespace Microsoft.Extensions.Hosting;

public static class Extensions
{
    public static void AddAppConfiguration(this IHostApplicationBuilder builder)
    {

#if DEBUG
        DefaultAzureCredentialOptions credentialOptions = new()
        {
            Diagnostics =
                {
                    LoggedHeaderNames = { "x-ms-request-id" },
                    LoggedQueryParameters = { "api-version" },
                    IsLoggingContentEnabled = true
                },
            ExcludeSharedTokenCacheCredential = true,
            ExcludeAzurePowerShellCredential = true,
            ExcludeVisualStudioCodeCredential = true,
            ExcludeEnvironmentCredential = true,
            ExcludeInteractiveBrowserCredential = true,
            ExcludeAzureCliCredential = false,
            ExcludeManagedIdentityCredential = false,
            ExcludeVisualStudioCredential = false
        };
#elif RELEASE
        string? managedIdentityClientId = builder.Configuration["ManagedIdentityClientId"];

            DefaultAzureCredentialOptions credentialOptions = new()
            {
                ManagedIdentityClientId = managedIdentityClientId,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeAzurePowerShellCredential = true,
                ExcludeVisualStudioCodeCredential = true,
                ExcludeEnvironmentCredential = true,
                ExcludeInteractiveBrowserCredential = true,
                ExcludeAzureCliCredential = false,
                ExcludeManagedIdentityCredential = false,
                ExcludeVisualStudioCredential = false
            };
#endif
#if RELEASE

        DefaultAzureCredential credential = new(credentialOptions);
        string endpoint = builder.Configuration.GetConnectionString("CodebreakerAppConfiguration") ?? throw new InvalidOperationException("Could not read AzureAppConfiguration");

        try
        {
            builder.Configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect(new Uri(endpoint), credential)
                    .Select("BotService*", labelFilter: LabelFilter.Null)
                    .Select("BotService*", builder.Environment.EnvironmentName);
            });
        }
        catch (Exception ex)
        {

        }

#endif
    }

    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.UseServiceDiscovery();
        });

        return builder;
    }

    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddRuntimeInstrumentation()
                       .AddBuiltInMeters();
            })
            .WithTracing(tracing =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    // We want to view all traces in development
                    tracing.SetSampler(new AlwaysOnSampler());
                }
                tracing.AddSource("Codebreaker.GameAPIs.Client", "Codebreaker.GameAPIs")
                       .AddAspNetCoreInstrumentation()
                       .AddGrpcClientInstrumentation()
                       .AddHttpClientInstrumentation();
            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        // TODO: what's the strategy using the OLTP exporter?
        // note here: not supported https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-configuration?tabs=aspnetcore
        // but it's configured by default with .NET Aspire

        bool useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
        }
#if PROMETHEUS 
        builder.Services.AddOpenTelemetry()
           .WithMetrics(metrics => metrics.AddPrometheusExporter());
#else
        builder.Services.AddOpenTelemetry()
           .UseAzureMonitor(options =>
           {
               options.ConnectionString = builder.Configuration["ApplicationInsightsConnectionString"];
           });
#endif
        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
#if PROMETHEUS
        app.MapPrometheusScrapingEndpoint();
#endif

        // All health checks must pass for app to be considered ready to accept traffic after starting
        app.MapHealthChecks("/health");

        // Only health checks tagged with the "live" tag must pass for app to be considered alive
        app.MapHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });

        return app;
    }

    private static MeterProviderBuilder AddBuiltInMeters(this MeterProviderBuilder meterProviderBuilder) =>
        meterProviderBuilder.AddMeter(
            "Codebreaker.Games",
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "Microsoft.EntityFrameworkCore",
            "System.Net.Http");
}
