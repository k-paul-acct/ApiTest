namespace AspTest.Data.Models;

public class UserGroup
{
    public int Id { get; set; }

    public required string Code { get; set; }

    public string? Description { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}
