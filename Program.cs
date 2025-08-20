using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using MvcMovie.Models;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// ✅ Ajout obligatoire pour QuestPDF Community License
QuestPDF.Settings.License = LicenseType.Community;

// ✅ Configuration de la base de données MySQL
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34))
    ));
// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MvcMovieContext>()
    .AddDefaultTokenProviders();


// ✅ Ajout des services MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ Initialisation de la base avec des données (SeedData)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);

    var context = services.GetRequiredService<MvcMovieContext>();
    DbInitializer.Initialize(context);
}

// ✅ Configuration du pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();


// ✅ Route par défaut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
