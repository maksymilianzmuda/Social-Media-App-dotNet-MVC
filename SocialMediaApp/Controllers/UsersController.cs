using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Interfaces;
using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepository _userRepository;
        public UsersController(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UsersViewModel> result = new List<UsersViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UsersViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Distance = user.Distance,
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult>Detail(string id)
        {
            var user = await _userRepository.GetUserId(id);
            var userDetailViewModel = new UsersDetailViewModel()
            {
                //Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Distance = user.Distance,
                

            };
            return View(userDetailViewModel);
        }
    }
}
