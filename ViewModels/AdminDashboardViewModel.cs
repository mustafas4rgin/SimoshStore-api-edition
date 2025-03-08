using App.Data.Entities;

namespace SimoshStore;

public class AdminDashboardViewModel
{
    public int TotalCategories { get; set; }
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalUsers { get; set; }
    public List<OrderEntity> LatestOrders { get; set; }
    public IEnumerable<ProductCommentEntity> LatestComments { get; set; }
    public IEnumerable<BlogEntity> LatestBlogs { get; set; } = null!;
}
