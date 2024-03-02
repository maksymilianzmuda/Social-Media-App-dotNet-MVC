using SocialMediaApp.Models;

namespace SocialMediaApp.Interfaces
{
    public interface IDashboardRepository
    {
         Task<List<Race>> GetAllUserRaces();
         Task<List<Club>> GetAllUserClubs();
    }
}
