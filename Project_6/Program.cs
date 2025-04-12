using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// Add MySQL configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        "Server=mysql-devdep-server.mysql.database.azure.com;Port=3306;Database=devdep;UserID=sqladmin;Password=Database123@;SslMode=Required;SslCa=DigiCertGlobalRootG2.crt.pem;",
        ServerVersion.AutoDetect("Server=mysql-devdep-server.mysql.database.azure.com;Port=3306;Database=devdep;UserID=sqladmin;Password=Database123@;")
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
