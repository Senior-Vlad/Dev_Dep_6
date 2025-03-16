namespace Project_6.Models;

public class RegistrationToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }
}
