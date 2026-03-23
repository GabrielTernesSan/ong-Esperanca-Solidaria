using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ong.Application;
using Ong.Application.Requests;
using Ong.Infra;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddInfraLayer(builder.Configuration);
builder.Services.AddApplicationLayer();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

#region Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT com prefixo 'Bearer '"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
#endregion

#region Authentication & Authorization
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("GestorONGPolicy", policy => policy.RequireRole("GestorONG"));
#endregion

var app = builder.Build();

app.MapPost("/auth/register", async ([FromBody] RegisterRequest request, IMediator mediator) =>
{
    var result = await mediator.Send(request);

    return result.HasErrors
        ? Results.BadRequest(result)
        : Results.Ok(result);
});

app.MapPost("/auth/login", async ([FromBody] LoginRequest request, IMediator mediator) =>
{
    var result = await mediator.Send(request);

    return result.HasErrors
        ? Results.Unauthorized()
        : Results.Ok(result);
});

app.MapPost("/donations/{campaingId}", async (Guid campaingId, [FromBody] DonationRequest request, IMediator mediator) =>
{
    request.CampaignId = campaingId;

    var result = await mediator.Send(request);

    return result.HasErrors
        ? Results.BadRequest(result)
        : Results.Ok(result);
}).AllowAnonymous();

#region Middleware Pipeline
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
