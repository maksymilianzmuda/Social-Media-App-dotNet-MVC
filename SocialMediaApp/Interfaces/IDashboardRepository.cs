using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Models;

namespace SocialMediaApp.Interfaces
{
    public interface IDashboardRepository
    {
         Task<List<Race>> GetAllUserRaces();
         Task<List<Club>> GetAllUserClubs();
         Task<AppUser> GetUserById(string id);
    }
}
