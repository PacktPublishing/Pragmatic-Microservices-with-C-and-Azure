﻿using Azure.Identity;

using Codebreaker.Data.Cosmos;
using Codebreaker.Ranking.Data;
using Codebreaker.Ranking.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace Codebreaker.Live;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddKeyedAzureBlobClient("checkpoints");

        builder.AddAzureEventProcessorClient("codebreakerevents", settings =>
        {
            settings.EventHubName = "games";
            settings.BlobClientServiceKey = "checkpoints";
        });

        builder.Services.AddDbContextFactory<RankingsContext>(options =>
        {
            string connectionString = builder.Configuration.GetConnectionString("codebreakercosmos") ?? throw new InvalidOperationException("Could not read the Cosmos connection-string");
            options.UseCosmos(connectionString, "codebreaker");
        });

        builder.EnrichCosmosDbContext<RankingsContext>();
    }

    public static WebApplication MapApplicationEndpoints(this WebApplication app)
    {
        app.MapRankingEndpoints();
        return app;
    }

    private static bool s_IsDatabaseUpdateComplete = false;
    internal static bool IsDatabaseUpdateComplete
    {
        get => s_IsDatabaseUpdateComplete;
    }

    public static async Task CreateOrUpdateDatabaseAsync(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            // TODO: update with .NET Aspire Preview 4
            var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<RankingsContext>>();
            using var context = await factory.CreateDbContextAsync();

            bool created = await context.Database.EnsureCreatedAsync();
            app.Logger.LogInformation("Database created: {created}", created);
            
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "Error updating database");
            throw;
        }

        s_IsDatabaseUpdateComplete = true;
    }
}
