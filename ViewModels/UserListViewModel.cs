namespace SimoshStore;

public class UserListViewModel
{
    public List<UserViewModel> Users { get; set; }
    public int TotalUsersCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}
