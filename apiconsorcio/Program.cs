using Microsoft.EntityFrameworkCore;
using apiconsorcio.Data;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,               // quantas vezes tentar de novo
            maxRetryDelay: TimeSpan.FromSeconds(20), // tempo máximo entre tentativas
            errorNumbersToAdd: null)
    ));

builder.Services.AddDbContext<CampanhaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,               // quantas vezes tentar de novo
            maxRetryDelay: TimeSpan.FromSeconds(20), // tempo máximo entre tentativas
            errorNumbersToAdd: null)
    ));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()   // Ou especifique "http://localhost:5500" se estiver usando um servidor local
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
});

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
