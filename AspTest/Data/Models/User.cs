namespace AspTest.Data.Models;

public class User
{
    public Guid Id { get; set; }

    public required string Login { get; set; }

    public string Password { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int UserGroupId { get; set; }

    public int UserStateId { get; set; }

    public UserGroup UserGroup { get; set; } = null!;

    public UserState UserState { get; set; } = null!;
}
