namespace AspTest.Contracts.Responses.Dto;

public class UserDto
{
    public required string Login { get; set; }

    public DateTime CreatedDate { get; set; }

    public required UserGroupDto UserGroup { get; set; }

    public required UserStateDto UserState { get; set; }
}