using System.Diagnostics;
using AspTest.Data;
using AspTest.Data.Models;
using AspTest.Services.UserService.Errors;
using AspTest.Services.UserService.Results;
using AspTest.Types;
using Functional;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspTest.Services.UserService;

public class UserService : IUserService
{
    private readonly ApiDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(ApiDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<User, RegisterUserError>> RegisterUser(string login, string password, string groupCode)
    {
        var sw = new Stopwatch();
        sw.Start();

        if (await _context.Users.AnyAsync(x => x.Login == login))
        {
            return new Error<RegisterUserError>(RegisterUserError.LoginTaken);
        }

        if (groupCode == UserGroupCode.Admin &&
            await _context.Users.AnyAsync(x => x.UserGroup.Code == UserGroupCode.Admin))
        {
            return new Error<RegisterUserError>(RegisterUserError.CannotBeAdmin);
        }

        var group = await _context.UserGroups.FirstOrDefaultAsync(x => x.Code == groupCode);
        var state = await _context.UserStates.FirstOrDefaultAsync(x => x.Code == UserStateCode.Active);

        var user = new User
        {
            Login = login,
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            UserGroupId = group!.Id,
            UserStateId = state!.Id
        };
        user.Password = _passwordHasher.HashPassword(user, password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        while (sw.Elapsed < TimeSpan.FromSeconds(5))
        {
        }

        return new Ok<User>(user);
    }

    public async Task<DeleteUserResult> DeleteUser(string login)
    {
        var user = await GetUser(login);
        if (user is null) return DeleteUserResult.NotFound;
        if (user.UserState.Code == UserStateCode.Blocked) return DeleteUserResult.AlreadyDeleted;

        var deleted = await _context.UserStates.FirstOrDefaultAsync(x => x.Code == UserStateCode.Blocked);

        user.UserStateId = deleted!.Id;

        await _context.SaveChangesAsync();

        return DeleteUserResult.Success;
    }

    public async Task<User?> GetUser(string login)
    {
        return await _context.Users
            .Include(x => x.UserState)
            .Include(x => x.UserGroup)
            .FirstOrDefaultAsync(x => x.Login == login);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users
            .AsNoTracking()
            .Include(x => x.UserState)
            .Include(x => x.UserGroup)
            .ToListAsync();
    }
}