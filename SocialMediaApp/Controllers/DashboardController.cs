using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Data;
using SocialMediaApp.Interfaces;
using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Controllers
{
    public class DashboardController : Controller
    {
        //dependency injection
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPhotoService _photoService;
        public DashboardController(IDashboardRepository dashboardRepository, 
            IHttpContextAccessor contextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _contextAccessor = contextAccessor;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }
        
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _contextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if(user == null)
            {
                return View("ERROR");
            }
            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                Distance = user.Distance,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
                
            };
            return View(editUserViewModel);
        }


    }
}
