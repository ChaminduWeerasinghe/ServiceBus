
using Api.Services;
using Microsoft.Azure.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("ServiceBus:ConnectionString");
var connectionEntryPath = builder.Configuration.GetConnectionString("ServiceBus:TopicName");

builder.Services.AddSingleton<ITopicClient>(x=>new TopicClient(connectionString,connectionEntryPath));
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();
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