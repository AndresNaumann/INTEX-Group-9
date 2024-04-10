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

            if (info != null)
            {
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
            } else
            {
                var tempModel = new UserInfoViewModel
                {
                    UserId = user.Id,
                    CustomerId = 0,
                    CustomerFirstName = null,
                    CustomerLastName = null,
                    Roles = await GetUserRoles(user),
                    Email = null,
                    Phone = null
                };

                return View(tempModel);
            }
 
        }

        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            var use = await _userManager.GetRolesAsync(user);
            if (use == null)
            {
                return null;
            } else
            {
                return new List<string>(await _userManager.GetRolesAsync(user));
            }
           
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

        // FOR EVERYONE TO UPDATE THEIR INFO

        [HttpGet]
        public async Task<IActionResult> EditSelf()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            string userId = currentUser?.Id;

            var currentCustomer = _repo.GetCustomerByUserId(userId);
       
            return View(currentCustomer);
        }

        [HttpPost]
        public IActionResult EditSelf(Customer customer)
        {
            var userToEdit = _repo.GetCustomerById(customer.CustomerId);

            if (userToEdit == null)
            {
                return NotFound();
            } else
            {
                _repo.UpdateUser(customer.CustomerId);
                _repo.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            //var result = await _userManager.UpdateAsync(userToEdit);
            //if (result.Succeeded)
            //{
            //    return RedirectToAction("AdminUsers", "Home");
            //}

            //return RedirectToAction("Index", "Home");
        }
    }
}
