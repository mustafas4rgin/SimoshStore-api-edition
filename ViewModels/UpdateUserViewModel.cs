namespace SimoshStore;
public class UpdateUserViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public bool Enabled { get; set; }
    public string Address { get; set; } = string.Empty;
}