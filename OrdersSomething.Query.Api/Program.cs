using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using OrdersSomething.Core.Events;
using OrdersSomething.Core.Middleware;
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

// 4. Mapster Configuration (GLOBAL)
// TypeAdapterConfig<Devices, DeviceDto>
//     .NewConfig()
//     .Map(dest => dest.PropertyId, src => src.PropertiesId);

// 5. MassTransit + Redpanda (Kafka Consumer)
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

    x.AddRider(rider =>
    {
        rider.AddConsumer<PropertyUpsertedConsumer>();
        rider.AddConsumer<PropertyDeletedConsumer>();
        rider.AddConsumer<DeviceUpsertedConsumer>();
        rider.AddConsumer<DeviceDeletedConsumer>();
        rider.AddConsumer<DeviceListeningChangedConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host("localhost:29092");

            k.TopicEndpoint<PropertyUpsertedEvent>("property-upserted-topic", "query-service-group", e =>
            {
                e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
                e.ConfigureConsumer<PropertyUpsertedConsumer>(context);
            });

            k.TopicEndpoint<DeviceUpsertedEvent>("device-upserted-topic", "query-service-group", e =>
            {
                e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
                e.ConfigureConsumer<DeviceUpsertedConsumer>(context);
            });

            k.TopicEndpoint<PropertyDeletedEvent>("property-deleted-topic", "query-service-group", e =>
            {
                e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
                e.ConfigureConsumer<PropertyDeletedConsumer>(context);
            });

            k.TopicEndpoint<DeviceDeletedEvent>("device-deleted-topic", "query-service-group", e =>
            {
                e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
                e.ConfigureConsumer<DeviceDeletedConsumer>(context);
            });

            k.TopicEndpoint<DeviceListeningChangedEvent>("device-listening-changed-topic", "query-service-group", e =>
            {
                e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
                e.ConfigureConsumer<DeviceListeningChangedConsumer>(context);
            });
        });
    });
});

// 6. Konfiguracja CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// 7. Error handler (Middleware)
app.UseMiddleware<ExceptionHandlingMiddleware>();

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
