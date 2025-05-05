using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_6.Models;

public class ZgloszenieStatus
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //... auto-increment
    public int Id { get; set; }

    public string Status { get; set; } // status (Nowe,W trakcie, Zamkniete)
    public DateTime DataZmiany { get; set; } // date of status change
    public int ZgloszenieId { get; set; } // outer key to Zgloszenie table
    public Zgloszenie Zgloszenie { get; set; } // navigational property to Zgloszenie table
}
