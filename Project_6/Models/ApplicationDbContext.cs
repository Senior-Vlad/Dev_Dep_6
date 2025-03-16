// using Microsoft.EntityFrameworkCore;

// public class ApplicationDbContext : DbContext
// {
//     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//         : base(options) { }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         if (!optionsBuilder.IsConfigured)
//         {
//             optionsBuilder.UseMySQL("Server=mysql-devdep-server.mysql.database.azure.com;Port=3306;Database=devdep;UserID=sqladmin;Password=Database123@;SslMode=Required;SslCa=~/Views/Shared/DigiCertGlobalRootG2.crt.pem;");
//         }
//     }
// }
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}