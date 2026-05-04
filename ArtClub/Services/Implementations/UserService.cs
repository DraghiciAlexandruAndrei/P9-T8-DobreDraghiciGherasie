using ArtClub.DataAccess;
using ArtClub.Models.Entities;
using ArtClub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtClub.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(User user, string password)
        {
            // Verificăm dacă email-ul este deja folosit
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            // Într-o aplicație reală, aici am folosi BCrypt pentru Password Hashing
            user.PasswordHash = password; // Simulare simplă

            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return false;

            // Verificare simplă (recomandat: hash comparison)
            return user.PasswordHash == password;
        }

        public async Task<bool> CheckUserStatusAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.IsActive ?? false;
        }
    }
}