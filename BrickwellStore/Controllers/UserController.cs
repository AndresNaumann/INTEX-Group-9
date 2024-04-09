using BrickwellStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrickwellStore.Controllers
{
    public class UserController : Controller
    {
        private UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
    }
}
