using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.user.Downloads.Dev_Dep_6.Project_6.Models
{
    public class Ogloszenie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string CreateByRole { get; set; } = "Secretariate";
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;



    }
}
