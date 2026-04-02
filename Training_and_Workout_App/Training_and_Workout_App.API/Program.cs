using Microsoft.OpenApi.Models;
using Training_and_Workout_App.DataAccess.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Training and Workout App API",
        Version = "v1",
        Description = "Documentatie Swagger pentru backend-ul aplicatiei."
    });
});
builder.Services.AddDataAccess(builder.Configuration);

var app = builder.Build();
var isDevelopment = app.Environment.IsDevelopment();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Training and Workout App API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () =>
{
    if (isDevelopment)
    {
        return Results.Redirect("/swagger");
    }

    return Results.Ok(new
    {
        message = "Training and Workout App API is running"
    });
}).ExcludeFromDescription();

app.Run();
