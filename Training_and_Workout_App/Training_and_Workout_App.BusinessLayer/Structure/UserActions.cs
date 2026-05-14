using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Models;
using Training_and_Workout_App.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class UserActions(ApplicationDbContext context, ITokenActions tokenActions, IConfiguration configuration) : IUserActions
{
    private readonly PasswordHasher<User> _hasher = new();

    public async Task<UserResponseDto> RegisterAsync(UserRegisterDto dto)
    {
        var user = new User
        {
            FullName = dto.FullName,
            Username = dto.Username,
        };
        // Hash parola inainte de salvare — nu se stocheaza niciodata in plaintext
        user.Password = _hasher.HashPassword(user, dto.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserResponseDto { Id = user.Id, FullName = user.FullName, Username = user.Username };
    }

    public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user is null)
            throw new Exception("Invalid credentials");

        // Verifica parola fata de hash-ul stocat in DB
        var verification = _hasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (verification == PasswordVerificationResult.Failed)
            throw new Exception("Invalid credentials");

        var expireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!);
        var token = tokenActions.GenerateToken(user.Id, user.FullName, user.Role.ToString());

        return new AuthResponseDto
        {
            UserId    = user.Id,
            FullName  = user.FullName,
            Role      = user.Role.ToString(),
            Token     = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expireMinutes)
        };
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null) return null;

        return new UserResponseDto { Id = user.Id, FullName = user.FullName, Username = user.Username };
    }

    public async Task<UserResponseDto?> GetMeAsync(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null) return null;

        return new UserResponseDto { Id = user.Id, FullName = user.FullName, Username = user.Username };
    }
}
