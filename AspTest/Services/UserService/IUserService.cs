using AspTest.Data.Models;
using AspTest.Services.UserService.Errors;
using AspTest.Services.UserService.Results;
using Functional;

namespace AspTest.Services.UserService;

public interface IUserService
{
    public Task<Result<User, RegisterUserError>> RegisterUser(string login, string password, string groupCode);

    public Task<DeleteUserResult> DeleteUser(string login);

    public Task<User?> GetUser(string login);
    
    public Task<IEnumerable<User>> GetAllUsers();
}