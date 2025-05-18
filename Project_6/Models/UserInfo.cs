using System;
using System.ComponentModel.DataAnnotations;

namespace Project_6.Models
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [MaxLength(20)]
        [RegularExpression("^[A-Za-zÀ-ÿżźćńółęąśŻŹĆĄŚĘŁÓŃ\\s'-]+$", ErrorMessage = "Dozwolone są tylko znaki alfabetyczne.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MaxLength(20)]
        [RegularExpression("^[A-Za-zÀ-ÿżźćńółęąśŻŹĆĄŚĘŁÓŃ\\s'-]+$", ErrorMessage = "Dozwolone są tylko znaki alfabetyczne.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Numer komórkowy jest wymagany.")]
        [Phone(ErrorMessage = "Nieprawidłowy format Numeru komórkowego.")]
        [MaxLength(10)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Dozwolone są tylko cyfry.")]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[1-9]+$", ErrorMessage = "Dozwolone są tylko cyfry.")]
        public string? YearOfStudy { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[A-Za-zÀ-ÿżźćńółęąśŻŹĆĄŚĘŁÓŃ\\s'-]+$", ErrorMessage = "Dozwolone są tylko znaki alfabetyczne.")]
        public string? Faculty { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[A-Za-zÀ-ÿżźćńółęąśŻŹĆĄŚĘŁÓŃ\\s'-]+$", ErrorMessage = "Dozwolone są tylko znaki alfabetyczne.")]
        public string? Major { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
        public override string ToString()
        {
            return $"UserId: {UserId}";
        }
    }
}
