using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using OrdersSomething.Core.Events;
using OrdersSomething.Query.Api;
using OrdersSomething.Query.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

// 1. Serwisy kontrolerów i Swaggera
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Query API", Version = "v1" });
});

// 2. DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// 4. MassTransit + Redpanda (Kafka Consumer)
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

    x.AddRider(rider =>
    {
        rider.AddConsumer<PropertyUpsertedConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host("localhost:29092");

            k.TopicEndpoint<PropertyUpsertedEvent>("property-upserted-topic", "query-service-group", e =>
            {
                e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
                e.ConfigureConsumer<PropertyUpsertedConsumer>(context);
            });
        });
    });
});

// 5. Konfiguracja CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll"); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Query API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace OrdersSomething.Query.Api
{
    public partial class Program { }
}