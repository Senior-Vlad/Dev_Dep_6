using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Users.user.Downloads.Dev_Dep_6.Project_6.Controllers
{
    public class ZgloszenieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZgloszenieController(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
