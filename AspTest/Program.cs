using AspTest.Contracts.Requests.Models;
using AspTest.Contracts.Responses.Dto;
using AspTest.Contracts.Responses.Mapping;
using AspTest.Data;
using AspTest.Data.Models;
using AspTest.Extensions;
using AspTest.Services.UserService;
using AspTest.Services.UserService.Results;
using FluentValidation;
using Functional;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

//
// Builder configuration.
//
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDbContext<ApiDbContext>(o => o.UseNpgsql(builder.Configuration["ConnectionString"]));

builder.Services.AddScoped<IUserService, UserService>();

builder.AddValidation();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

//
// App configuration.
//
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddDbSeeds();

//
// Mapping.
//
app.MapPost("/users/create", async (Register model, IValidator<Register> validator, IUserService userService) =>
{
    var validationErrors = Validate(model, validator);
    if (validationErrors is not null) return Results.BadRequest(validationErrors);

    var res = await userService.RegisterUser(model.Login, model.Password, model.GroupCode);
    return res.Status switch
    {
        ResultStatus.Ok => Results.Ok(UserMapper.ToDto(res.Value, res.Value.UserState, res.Value.UserGroup)),
        _ => Results.BadRequest(new { Error = res.Error.ToString(), Code = res.Error })
    };
});

app.MapGet("/users", async (IUserService userService) =>
{
    var users = await userService.GetAllUsers();
    var dto = users.Select(x => new UserDto
    {
        Login = x.Login,
        CreatedDate = x.CreatedDate,
        UserGroup = new UserGroupDto
        {
            Code = x.UserGroup.Code,
            Description = x.UserGroup.Description
        },
        UserState = new UserStateDto
        {
            Code = x.UserState.Code,
            Description = x.UserState.Description
        }
    });
    return Results.Ok(new { Items = dto });
});

app.MapGet("/users/{login}", async (string login, IUserService userService) =>
{
    var user = await userService.GetUser(login);
    return user is null
        ? Results.BadRequest(new { Error = "User not found" })
        : Results.Ok(UserMapper.ToDto(user, user.UserState, user.UserGroup));
});

app.MapDelete("/users/{login}", async (string login, IUserService userService) =>
{
    var res = await userService.DeleteUser(login);
    return res switch
    {
        DeleteUserResult.Success => Results.Ok(),
        _ => Results.BadRequest(new { Error = res.ToString(), Code = res })
    };
});

//
// Starting the app.
//
app.Run();

//
// Helper methods.
//
static IResult? Validate<TModel>(TModel model, IValidator<TModel> validator)
{
    var validationResult = validator.Validate(model);
    if (validationResult.IsValid) return null;

    var errors = validationResult.Errors.Select(x => new
    {
        Field = x.PropertyName, Message = x.ErrorMessage
    });

    return Results.BadRequest(new
    {
        Error = "bad_request", ErrorDescription = "One or more validation errors occurred", Errors = errors
    });
}