using System;
using System.ComponentModel.DataAnnotations;

namespace Project_6.Models
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string YearOfStudy { get; set; }

        public string Faculty { get; set; }

        public string Major { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
        public override string ToString()
        {
            return $"UserId: {UserId}";
        }
    }
}
