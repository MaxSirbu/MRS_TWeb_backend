using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Models.User;
using Training_and_Workout_App.DataAccess.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class UserActions(ApplicationDbContext context, ITokenAction tokenActions, IConfiguration configuration)
{
    private readonly PasswordHasher<UserData> _hasher = new();

    public async Task<UserResponseDto> RegisterAsync(UserRegisterDto dto)
    {
        var normalizedEmail = NormalizeEmail(dto.Email);
        var emailExists = await context.Users
            .AnyAsync(user => user.Email.ToLower() == normalizedEmail);

        if (emailExists)
            throw new InvalidOperationException("An account with this email already exists.");

        var user = new UserData
        {
            FullName = dto.FullName.Trim(),
            Email = normalizedEmail,
        };
        // Hash parola inainte de salvare — nu se stocheaza niciodata in plaintext
        user.Password = _hasher.HashPassword(user, dto.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserResponseDto { Id = user.Id, FullName = user.FullName, Email = user.Email };
    }

    public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
    {
        var normalizedEmail = NormalizeEmail(dto.Email);
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials");

        if (user.Blocked)
            throw new UnauthorizedAccessException("Account blocked");

        // Verifica parola fata de hash-ul stocat in DB
        var verification = _hasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (verification == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid credentials");

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.Password = _hasher.HashPassword(user, dto.Password);
            await context.SaveChangesAsync();
        }

        var expireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"] ?? "60");
        var token = tokenActions.GenerateToken(user.Id, user.FullName, user.Role.ToString());

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expireMinutes)
        };
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null) return null;

        return new UserResponseDto { Id = user.Id, FullName = user.FullName, Email = user.Email };
    }

    public async Task<UserResponseDto?> GetMeAsync(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null) return null;

        return new UserResponseDto { Id = user.Id, FullName = user.FullName, Email = user.Email };
    }

    // ── Pasul 2: update date proprii ──────────────────────────────────────────
    public async Task<UserResponseDto?> UpdateMeAsync(int userId, UserUpdateDto dto)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null) return null;

        if (!string.IsNullOrWhiteSpace(dto.FullName))
            user.FullName = dto.FullName.Trim();
        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            var normalizedEmail = NormalizeEmail(dto.Email);
            var emailExists = await context.Users.AnyAsync(existing =>
                existing.Id != userId && existing.Email.ToLower() == normalizedEmail);
            if (emailExists)
                throw new InvalidOperationException("An account with this email already exists.");

            user.Email = normalizedEmail;
        }

        await context.SaveChangesAsync();
        return new UserResponseDto { Id = user.Id, FullName = user.FullName, Email = user.Email };
    }

    // ── Pasul 3: admin — listare toti userii ──────────────────────────────────
    public async Task<List<UserAdminResponseDto>> GetAllAsync()
    {
        return await context.Users
            .OrderBy(u => u.Id)
            .Select(u => new UserAdminResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role.ToString(),
                Blocked = u.Blocked,
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
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Blocked = user.Blocked,
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
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Blocked = user.Blocked,
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

    private static string NormalizeEmail(string email) =>
        email.Trim().ToLowerInvariant();
}
