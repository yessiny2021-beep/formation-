using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using Microsoft.AspNetCore.Identity;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ------------------
// 🔹 QuestPDF License
// ------------------
QuestPDF.Settings.License = LicenseType.Community;

// ------------------
// 🔹 Services
// ------------------
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34))
    )
);

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<MvcMovieContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ------------------
// 🔹 Seed rôles et admin
// ------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    // Crée les rôles s'ils n'existent pas
    string[] roles = { "Admin", "Employe" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Crée un admin par défaut
    string adminEmail = "admin@formation.com";
    string adminPassword = "Admin123!";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// ------------------
// 🔹 Commande CLI pour changer le rôle d’un utilisateur
// ------------------
if (args.Length == 3 && args[0].ToLower() == "changerole")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string email = args[1];
    string newRole = args[2];

    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        Console.WriteLine($"Utilisateur {email} introuvable !");
        return;
    }

    // Crée le rôle si nécessaire
    if (!await roleManager.RoleExistsAsync(newRole))
    {
        Console.WriteLine($"Le rôle {newRole} n'existe pas. Création...");
        await roleManager.CreateAsync(new IdentityRole(newRole));
    }

    // Supprime tous les rôles existants
    var currentRoles = await userManager.GetRolesAsync(user);
    await userManager.RemoveFromRolesAsync(user, currentRoles);

    // Ajoute le nouveau rôle
    await userManager.AddToRoleAsync(user, newRole);

    Console.WriteLine($"Le rôle de {email} a été changé en {newRole}.");
    return; // Termine l'application après la commande
}

// ------------------
// 🔹 Middleware
// ------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ------------------
// 🔹 Routes
// ------------------
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
