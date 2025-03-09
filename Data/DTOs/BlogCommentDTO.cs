namespace SimoshStore;
public class BlogCommentDTO
{
    public int BlogId { get; set; }   
    public string Name { get; set; }  = "Anonymous";
    public string Email { get; set; }  
    public string Comment { get; set; } 
    public int UserId { get; set; }
}
