using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using User.Services;
using User.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Default Connection is not set");
}
builder.Services.AddScoped<DBConnectionFactory>(_ => new DBConnectionFactory(connectionString));


builder.Services.AddScoped<IDBService<UserItem>, UserDBService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
