public class Complaint
{
    public int Id { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public bool IsResolved { get; set; } = false;

// connection with User table
    public int UserId { get; set; }
    public User User { get; set; }
}
