using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Message
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SenderId { get; set; }

    [ForeignKey("SenderId")]
    public User Sender { get; set; }

    public int? ReceiverId { get; set; }

    [ForeignKey("ReceiverId")]
    public User Receiver { get; set; }

    public int? GroupId { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}


