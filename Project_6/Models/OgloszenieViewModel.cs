using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_6.Models
{
    public class OgloszenieViewModel
    {
        [Required(ErrorMessage = "Tytul jest wymagany.")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Wiadomość jest wymagana.")]
        [MaxLength(200)]
        public string Message { get; set; }
        public string CreateByRole { get; set; } = "Secretariate";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
