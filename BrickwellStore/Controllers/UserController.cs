using BrickwellStore.Data;
using BrickwellStore.Data.ViewModels;
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
  

        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Or handle accordingly
            }

            var roles = await _userManager.GetRolesAsync(user);


            var info = _repo.GetCustomerByUserId(user.Id);
            if (info == null)
            {
                var model = new Customer
                {
                    UserId = user.Id
                };

                _repo.AddUser(model); 
            }

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = await GetUserRoles(user)
            };

            var infoModel = new UserInfoViewModel
            {
                UserId = user.Id,
                CustomerId = info.CustomerId,
                CustomerFirstName = info.CustomerFirstName,
                CustomerLastName = info.CustomerFirstName,
                Roles = await GetUserRoles(user),
                Email = info.Email,
                Phone = info.Phone
            };

            return View(infoModel);
        }

        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
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

        [HttpGet]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var toBeAdmin = await _userManager.FindByIdAsync(id);

            return View(toBeAdmin);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAdmin(IdentityUser user)
        {
            var toBeAdmin = await _userManager.FindByIdAsync(user.Id);

            if (toBeAdmin == null)
            {
                return NotFound();
            }

            var result = await _userManager.AddToRoleAsync(toBeAdmin, "Admin"); ;
            if (result.Succeeded)
            {
                return RedirectToAction("AdminUsers", "Admin");
            }

            return RedirectToAction("AdminUsers", "Admin");
        }

        // Editing and Deleting User Actions are found in the Admin Controller
    }
}
