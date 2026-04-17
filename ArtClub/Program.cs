using Microsoft.EntityFrameworkCore;
using ArtClub.DataAccess; // Namespace-ul unde se află ApplicationDbContext
using ArtClub.Services.Interfaces;
using ArtClub.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurare Bază de Date (Entity Framework)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Înregistrarea Serviciilor (Dependency Injection)
// Fără astea, Controllerul va da eroare când va încerca să pornească
// 1. Mai întâi utilitarele de bază (care nu depind de nimeni)
builder.Services.AddScoped<INotificationService, NotificationService>();

// 2. Apoi serviciile care depind de Context sau de Notificări
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArtPieceService, ArtPieceService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// 3. La final, serviciile complexe care depind de cele de mai sus
// EventService are nevoie de INotificationService, deci e bine să fie după el.
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IFinanceService, FinanceService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Ordinea contează: întâi Autentificare, apoi Autorizare
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();