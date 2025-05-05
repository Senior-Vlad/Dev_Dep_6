public class ComplaintViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime SubmittedAt { get; set; }
    public bool IsResolved { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}
