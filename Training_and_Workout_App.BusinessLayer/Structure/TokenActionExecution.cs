using Training_and_Workout_App.BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Training_and_Workout_App.BusinessLayer.Core;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class TokenActionExecution(IConfiguration configuration)
    : TokenActions(configuration), ITokenAction
{
}
