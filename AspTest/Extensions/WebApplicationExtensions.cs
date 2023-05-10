using AspTest.Data;
using AspTest.Data.Models;
using AspTest.Types;

namespace AspTest.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication AddDbSeeds(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

        if (!context.UserGroups.Any(x => x.Code == UserGroupCode.Admin))
        {
            var group = new UserGroup
            {
                Code = UserGroupCode.Admin,
                Description = "N/A"
            };
            context.UserGroups.Add(group);
        }

        if (!context.UserGroups.Any(x => x.Code == UserGroupCode.User))
        {
            var group = new UserGroup
            {
                Code = UserGroupCode.User,
                Description = "N/A"
            };
            context.UserGroups.Add(group);
        }

        if (!context.UserStates.Any(x => x.Code == UserStateCode.Active))
        {
            var group = new UserState
            {
                Code = UserStateCode.Active,
                Description = "N/A"
            };
            context.UserStates.Add(group);
        }

        if (!context.UserStates.Any(x => x.Code == UserStateCode.Blocked))
        {
            var group = new UserState
            {
                Code = UserStateCode.Blocked,
                Description = "N/A"
            };
            context.UserStates.Add(group);
        }

        context.SaveChanges();

        return app;
    }
}