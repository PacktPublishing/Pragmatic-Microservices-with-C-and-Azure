﻿using Codebreaker.Data.SqlServer;
using Codebreaker.SqlServerMigration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ApiDbInitializer>();

builder.AddServiceDefaults();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(ApiDbInitializer.ActivitySourceName));

builder.AddSqlServerDbContext<GamesSqlServerContext>("codebreaker");

var app = builder.Build();

app.Run();
