using Hangfire;
using Hangfire.PostgreSql;
using OpenTelemetry.Metrics;
using Ong.Application;
using Ong.Application.Worker;
using Ong.Application.Worker.Configurations;
using Ong.Application.Worker.Jobs;
using Ong.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfraLayerForWorker(builder.Configuration);
builder.Services.AddApplicationLayer();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddRuntimeInstrumentation();
        metrics.AddPrometheusExporter();
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(connectionString);
    }));

builder.Services.AddApplicationWorkerLayer(builder.Configuration);

builder.Services.AddHangfireServer(options =>
{
    options.Queues = [Queues.OutboxPublisherJob, "default"];
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHangfireDashboard();

app.MapHealthChecks("/health");
app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");

using (var scope = app.Services.CreateScope())
{
    var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJob.AddOrUpdate<OutboxPublisherJob>(
        "processar-mensagens-outbox",
        job => job.ProcessAsync(),
        Cron.Minutely()
    );
}

app.Run();
