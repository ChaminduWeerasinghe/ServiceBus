using Microsoft.Azure.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ServiceBus:ConnectionString");
var topic = builder.Configuration.GetConnectionString("ServiceBus:TopicName");
var subscriptionName = builder.Configuration.GetConnectionString("ServiceBus:SubscriptionName");

builder.Services.AddSingleton<ISubscriptionClient>(x=>new SubscriptionClient(connectionString,topic,subscriptionName));

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