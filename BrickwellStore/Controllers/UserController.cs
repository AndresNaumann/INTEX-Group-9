using BrickwellStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrickwellStore.Controllers
{
    public class UserController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private ILegoRepository _repo;

        public UserController(UserManager<IdentityUser> userManager, ILegoRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }
        public IActionResult UserDetails(string id)
        {

            var user = _userManager.Users.FirstOrDefault(p => p.Id == id);
            if (user == null)
            {
                return NotFound(); // Or handle accordingly
            }
            return View(user);
        }

        public IActionResult CustomerDetails(string id)
        {

            var user = _repo.GetCustomerByUserId(id);
            if (user == null)
            {
                return NotFound(); // Or handle accordingly
            }
            return View(user);
        }
    }
}
