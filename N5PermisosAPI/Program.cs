using N5PermisosAPI.Data;
using Microsoft.EntityFrameworkCore;
using Nest;
using Confluent.Kafka;
using N5PermisosAPI.Models;
using N5PermisosAPI.DataAccess.Interfaces;
using N5PermisosAPI.DataAccess.Repositories;
using System.Reflection;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the database context
builder.Services.AddDbContext<N5PermisosAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("N5PermisosAPIContext")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPermisosRepository, PermisosRepository>();
builder.Services.AddScoped<ILogEvent, LogEvent>();
//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(N5PermisosAPI.CQRS.Queries.GetPermisosQuery).Assembly);


// Add configuratión Elasticsearch 
var settings = builder.Configuration.GetSection("Elasticsearch").Get<ElasticsearchSettings>();
var connectionSettings = new ConnectionSettings(new Uri(settings.Uri))
    .DefaultIndex("permisos");

// Add configuration Apache Kafka
var kafkaSettings = builder.Configuration.GetSection("Kafka").Get<KafkaSettings>();
var producerConfig = new ProducerConfig
{
    BootstrapServers = kafkaSettings.BootstrapServers
};

builder.Services.AddSingleton(producerConfig);

var client = new ElasticClient(connectionSettings);
builder.Services.AddSingleton<IElasticClient>(client);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
