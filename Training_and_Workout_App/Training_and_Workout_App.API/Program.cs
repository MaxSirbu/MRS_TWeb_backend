using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.BusinessLayer.Structure;
using Training_and_Workout_App.DataAccess.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ─── Autentificare JWT Bearer ──────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],

            ValidateAudience         = true,
            ValidAudience            = builder.Configuration["Jwt:Audience"],

            ValidateLifetime         = true,   // verifica "exp" la fiecare request

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

// ─── Swagger cu butonul "Authorize 🔒" ────────────────────────────────────
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Training and Workout App API",
        Version     = "v1",
        Description = "Documentatie Swagger pentru backend-ul aplicatiei."
    });

    // Definim schema de securitate Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Introdu token-ul JWT: Bearer &lt;token&gt;"
    });

    // Aplicam schema la toate endpoint-urile
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDataAccess(builder.Configuration);

// ─── BusinessLayer — inregistrare servicii ────────────────────────────────
builder.Services.AddScoped<ITokenService, TokenService>();   // JWT generation
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();
builder.Services.AddScoped<IMealPlanService, MealPlanService>();
builder.Services.AddScoped<IFoodItemService, FoodItemService>();
builder.Services.AddScoped<IMealDayEntryService, MealDayEntryService>();
builder.Services.AddScoped<IPlanActivationService, PlanActivationService>();
builder.Services.AddScoped<IPlanCompletionService, PlanCompletionService>();
builder.Services.AddScoped<IPlanCustomizationService, PlanCustomizationService>();
builder.Services.AddScoped<IUserPlanFavoriteService, UserPlanFavoriteService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IWorkoutTrackingService, WorkoutTrackingService>();
builder.Services.AddScoped<IQuestionnaireService, QuestionnaireService>();

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

// ─── Pipeline — ORDINEA CONTEAZA ──────────────────────────────────────────
app.UseAuthentication();   // 1. Citeste token, populeaza HttpContext.User
app.UseAuthorization();    // 2. Verifica [Authorize] si roluri

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

