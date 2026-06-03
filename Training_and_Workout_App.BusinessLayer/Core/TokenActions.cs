using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.FAQ;
using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Entities.Question;
using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Entities.WorkoutHistory;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Training_and_Workout_App.Domain.Models.DayPlan;
using Training_and_Workout_App.Domain.Models.Exercise;
using Training_and_Workout_App.Domain.Models.FAQ;
using Training_and_Workout_App.Domain.Models.FoodItem;
using Training_and_Workout_App.Domain.Models.MealDayEntry;
using Training_and_Workout_App.Domain.Models.MealPlan;
using Training_and_Workout_App.Domain.Models.PlanActivation;
using Training_and_Workout_App.Domain.Models.PlanCompletion;
using Training_and_Workout_App.Domain.Models.PlanCustomization;
using Training_and_Workout_App.Domain.Models.Question;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Models.User;
using Training_and_Workout_App.Domain.Models.UserPlanFavorite;
using Training_and_Workout_App.Domain.Models.UserProfile;
using Training_and_Workout_App.Domain.Models.WorkoutPlan;
using Training_and_Workout_App.Domain.Models.WorkoutTracking;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class TokenActions(IConfiguration configuration)
{
    public string GenerateToken(int userId, string fullName, string role)
    {
        // 1. Cheia simetrica — citita din configuratie, NICIODATA hardcodata
        var keyBytes = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // 2. Claims — informatiile puse in Payload
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // "sub"
            new Claim(ClaimTypes.Name, fullName),                    // "name"
            new Claim(ClaimTypes.Role, role)                         // "role" — folosit de [Authorize(Roles=...)]
        };

        // 3. Durata de expirare
        var expireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!);
        var expires = DateTime.UtcNow.AddMinutes(expireMinutes);

        // 4. Construim token-ul
        var token = new JwtSecurityToken(
            issuer:             configuration["Jwt:Issuer"],
            audience:           configuration["Jwt:Audience"],
            claims:             claims,
            expires:            expires,
            signingCredentials: credentials
        );

        // 5. Serializam in format Header.Payload.Signature
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
