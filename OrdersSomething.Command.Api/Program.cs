using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using OrdersSomething.Command.Api;
using OrdersSomething.Core.Events;
using OrdersSomething.Core.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Serwisy kontrolerów i Swaggera
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Command API", Version = "v1" });
});

// 2. Baza i MediatR
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// 3. MassTransit + Redpanda (Kafka)
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

    x.AddRider(rider =>
    {
        rider.AddProducer<PropertyUpsertedEvent>("property-upserted-topic");
        rider.AddProducer<PropertyDeletedEvent>("property-deleted-topic");

        rider.UsingKafka((context, k) =>
        {
            k.Host("localhost:29092");
        });
    });
});

// 4. Konfiguracja CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", p => 
        p.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());
});

var app = builder.Build();

// 5. Error handler (Middleware)
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Command API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace OrdersSomething.Command.Api
{
    public partial class Program { }
}
