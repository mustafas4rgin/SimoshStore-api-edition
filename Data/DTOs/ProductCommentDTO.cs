namespace SimoshStore;

public class ProductCommentDTO
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = null!;
    public int StarCount { get; set; }
}
