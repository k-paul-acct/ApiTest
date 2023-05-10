using AspTest.Contracts.Responses.Dto;
using AspTest.Data.Models;

namespace AspTest.Contracts.Responses.Mapping;

public static class UserMapper
{
    public static UserDto ToDto(User user, UserState state, UserGroup group)
    {
        var stateDto = new UserStateDto { Code = state.Code, Description = state.Description };
        var groupDto = new UserGroupDto { Code = group.Code, Description = group.Description };

        return new UserDto
        {
            Login = user.Login,
            UserGroup = groupDto,
            UserState = stateDto,
            CreatedDate = user.CreatedDate
        };
    }
}