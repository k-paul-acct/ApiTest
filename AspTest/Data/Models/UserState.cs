namespace AspTest.Data.Models;

public class UserState
{
    public int Id { get; set; }

    public required string Code { get; set; }

    public string? Description { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}
