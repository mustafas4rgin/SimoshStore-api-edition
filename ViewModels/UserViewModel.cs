namespace SimoshStore;

public class UserViewModel
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = default!;
    public int RoleId { get; set; }
    public bool Enabled { get; set; }
}
