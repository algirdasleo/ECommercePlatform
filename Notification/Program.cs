using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using Notification.Hubs;
using Notification.Models;
using Notification.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Default Connection is not set");
}
builder.Services.AddScoped<DBConnectionFactory>(_ => new DBConnectionFactory(connectionString));
builder.Services.AddScoped<IDBService<NotificationItem>, NotificationDBService>();
builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

app.MapHub<NotificationHub>("/notificationHub");

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
