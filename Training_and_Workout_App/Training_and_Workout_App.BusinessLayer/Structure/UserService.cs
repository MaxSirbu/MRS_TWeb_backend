using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Models;
using Training_and_Workout_App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class UserService(ApplicationDbContext context) : IUserService
{
    public async Task<UserResponseDto> RegisterAsync(UserRegisterDto dto)
    {
        var user = new User
        {
            FullName = dto.FullName,
            Password = dto.Password
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserResponseDto { Id = user.Id, FullName = user.FullName };
    }

    public async Task<UserResponseDto> LoginAsync(UserLoginDto dto)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.FullName == dto.FullName && u.Password == dto.Password);

        if (user is null) throw new Exception("Invalid credentials");
        
        return new UserResponseDto { Id = user.Id, FullName = user.FullName};
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null) return null;

        return new UserResponseDto { Id = user.Id, FullName = user.FullName};
    }

}