using SharedLibrary.Interfaces;
using SharedLibrary.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var databaseName = builder.Configuration["DatabaseName"];
var connectionStringTemplate = builder.Configuration.GetConnectionString("DatabaseTemplate");
// database template = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database={0}";
if (string.IsNullOrEmpty(connectionStringTemplate))
{
    throw new Exception("DatabaseTemplate is not set");
}
var connectionString = string.Format(connectionStringTemplate, databaseName);
builder.Services.AddScoped<DBConnectionFactory>(_ => new DBConnectionFactory(connectionString));


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
