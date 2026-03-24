using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ong.Application.Worker.Consumers;
using Ong.Application.Worker.Jobs;
using Ong.Infra;

namespace Ong.Application.Worker
{
    public static class ApplicationWorkerServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationWorkerLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<OngDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(DonationCreatedConsumer).Assembly);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(config["RabbitMq:Host"] ?? "localhost", h =>
                    {
                        h.Username(config["RabbitMq:Username"] ?? "guest");
                        h.Password(config["RabbitMq:Password"] ?? "guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<OutboxPublisherJob>();

            return services;
        }
    }
}