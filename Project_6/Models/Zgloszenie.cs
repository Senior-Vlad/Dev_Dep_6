namespace Project_6.Models;
public class Zgloszenie
{
    public int Id { get; set; }
    public string Tytul { get; set; }
    public string Tresc { get; set; }
    public DateTime DataDodania { get; set; }

    public int StudentId { get; set; } // outer key to User table
    public User? Student { get; set; } // Navigational property to User table
    public List<ZgloszenieStatus> Statusy { get; set; } = new(); // History of statuses
    public string AktualnyStatus => Statusy.LastOrDefault()?.Status ?? "Brak statusu";
    public int UserId { get; set; }  // Identificator of user, that sent a complaint
    public User? User { get; set; }      // Nawigational property to User table

}
