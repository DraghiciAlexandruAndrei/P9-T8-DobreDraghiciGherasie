using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using ArtClub.DataAccess;
using ArtClub.DataAccess.Interfaces;
using ArtClub.DataAccess.Repositories;
using ArtClub.Services.Interfaces;
using ArtClub.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurare Bază de Date (Entity Framework)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Înregistrarea REPOSITORY-URILOR (Data Access Layer)
// Acestea trebuie înregistrate pentru ca Serviciile să le poată folosi
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArtPieceRepository, ArtPieceRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// 3. Înregistrarea SERVICIILOR (Business Logic Layer)
// Înregistrăm mai întâi utilitarele independente
builder.Services.AddScoped<INotificationService, NotificationService>();

// Înregistrăm serviciile care folosesc Repository-urile de mai sus
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArtPieceService, ArtPieceService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IFinanceService, FinanceService>();

// EventService depinde de aproape toate celelalte, deci îl punem la final
builder.Services.AddScoped<IEventService, EventService>();

// 4. Configurare Servicii de Sistem (Sesiune și MVC)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

// 5. Autentificare pe bază de Cookie (fără ASP.NET Identity)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// 6. Politici de Autorizare bazate pe Claim-uri
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("MemberOrAdmin", policy =>
        policy.RequireClaim("Role", "Admin", "Member"));
});

var app = builder.Build();

// 7. Configurare Pipeline HTTP (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // necesar pentru CSS/JS/imagini
app.UseRouting();

// Sesiunea trebuie activată ÎNAINTE de Autentificare
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();