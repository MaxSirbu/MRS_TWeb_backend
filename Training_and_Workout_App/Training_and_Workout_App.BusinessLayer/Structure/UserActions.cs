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
            throw new UnauthorizedAccessException("Invalid credentials");

        // Verifica parola fata de hash-ul stocat in DB
        var verification = _hasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (verification == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid credentials");

        var expireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"] ?? "60");
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

    // ── Pasul 2: update date proprii ──────────────────────────────────────────
    public async Task<UserResponseDto?> UpdateMeAsync(int userId, UserUpdateDto dto)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null) return null;

        if (!string.IsNullOrWhiteSpace(dto.FullName))
            user.FullName = dto.FullName.Trim();
        if (!string.IsNullOrWhiteSpace(dto.Username))
            user.Username = dto.Username.Trim();

        await context.SaveChangesAsync();
        return new UserResponseDto { Id = user.Id, FullName = user.FullName, Username = user.Username };
    }

    // ── Pasul 3: admin — listare toti userii ──────────────────────────────────
    public async Task<List<UserAdminResponseDto>> GetAllAsync()
    {
        return await context.Users
            .OrderBy(u => u.Id)
            .Select(u => new UserAdminResponseDto
            {
                Id       = u.Id,
                FullName = u.FullName,
                Username = u.Username,
                Role     = u.Role.ToString(),
                Blocked  = u.Blocked,
            })
            .ToListAsync();
    }

    // ── Pasul 3: admin — schimbare rol ────────────────────────────────────────
    public async Task<UserAdminResponseDto?> SetRoleAsync(int id, SetRoleDto dto)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null) return null;

        if (Enum.TryParse<UserRole>(dto.Role, ignoreCase: true, out var parsed))
            user.Role = parsed;

        await context.SaveChangesAsync();
        return new UserAdminResponseDto
        {
            Id = user.Id, FullName = user.FullName, Username = user.Username,
            Role = user.Role.ToString(), Blocked = user.Blocked,
        };
    }

    // ── Pasul 3: admin — toggle blocare ───────────────────────────────────────
    public async Task<UserAdminResponseDto?> ToggleBlockedAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null) return null;

        user.Blocked = !user.Blocked;
        await context.SaveChangesAsync();
        return new UserAdminResponseDto
        {
            Id = user.Id, FullName = user.FullName, Username = user.Username,
            Role = user.Role.ToString(), Blocked = user.Blocked,
        };
    }

    // ── Pasul 3: admin — stergere user ────────────────────────────────────────
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null) return false;

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }
}
