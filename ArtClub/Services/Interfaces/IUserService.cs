using ArtClub.Models.Entities;

namespace ArtClub.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user, string password);
        Task<bool> AuthenticateAsync(string email, string password);
        Task<bool> CheckUserStatusAsync(int userId);
    }
}