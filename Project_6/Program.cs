using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();

// Add MySQL configuration - dev stage
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseMySql(
//         "Server=mysql-devdep-server.mysql.database.azure.com;Port=3306;Database=devdep;UserID=sqladmin;Password=Database123@;SslMode=Required;SslCa=DigiCertGlobalRootG2.crt.pem;",
//         ServerVersion.AutoDetect("Server=mysql-devdep-server.mysql.database.azure.com;Port=3306;Database=devdep;UserID=sqladmin;Password=Database123@;")
//     )
// );

// Add MySQL configuration - prod stage
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseMySql(
//         "Server=mysql-devdep-server.mysql.database.azure.com;Port=3306;Database=devdep;UserID=sqladmin;Password=Database123@;SslMode=Required;SslCa=C:\\home\\site\\wwwroot\\wwwroot\\certificates\\DigiCertGlobalRootG2.crt.pem;",
//         ServerVersion.AutoDetect("Server=mysql-devdep-server.mysql.database.azure.com;Port=3306;Database=devdep;UserID=sqladmin;Password=Database123@;")
//     )
// );
// Get the current environment
var env = builder.Environment;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (env.IsProduction() || env.IsDevelopment())
{
    // In production or development (Azure environment), use the full path to the certificate.
    var certPath = Path.Combine(env.ContentRootPath, "wwwroot", "certificates", "DigiCertGlobalRootG2.crt.pem");
    connectionString = connectionString.Replace("PATH_TO_CERT", certPath);
}
else
{
    // In local development, just use the certificate name or a relative path.
    connectionString = connectionString.Replace("PATH_TO_CERT", "DigiCertGlobalRootG2.crt.pem");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);


builder.Services.AddHttpContextAccessor();
// builder.Services.AddSession();
var app = builder.Build();
// app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapHub<ChatHub>("/chatHub");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(

        name: default,
        pattern: "{controller=Auth}/{action=Login}/{id?}").WithStaticAssets();



// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}")
//     .WithStaticAssets();

app.MapControllerRoute(
    name: "signup",
    pattern: "signup",
    defaults: new { controller = "Auth", action = "Signup" });

app.Run();
