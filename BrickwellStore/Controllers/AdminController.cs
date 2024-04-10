using BrickwellStore.Data;
using BrickwellStore.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BrickwellStore.Controllers
{
    public class AdminController : Controller
    {
        private ILegoRepository _repo;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(ILegoRepository temp, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repo = temp;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AdminUsers()
        //{
        //    var users = _userManager.Users.ToList();
        //    return View(users);
        //}
        public async Task<IActionResult> AdminUsers()
        {
            var users = _userManager.Users.ToList();
            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            return View(userRolesViewModel);
        }

        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        // ADDING A PRODUCT ------------------------------------------

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View("AddProduct");
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _repo.AddProduct(product);
            _repo.SaveChanges();

            return RedirectToAction("AdminProducts");
        }

        // EDITING ----------------------------------------------------

        // Edit a Customer/User

        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            var recordToEdit = _repo.GetCustomerById(id);

            return View(recordToEdit);
        }

        [HttpPost]
        public IActionResult EditCustomer(Customer updatedInfo)
        {
            _repo.UpdateUser(updatedInfo.CustomerId);
            _repo.SaveChanges();
            return RedirectToAction("AdminUsers");
        }

        // Edit a Product

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var recordToEdit = _repo.GetProductById(id);

            return View(recordToEdit);
        }

        [HttpPost]
        public IActionResult EditProduct(Product updatedInfo)
        {
            _repo.UpdateProduct(updatedInfo);
            _repo.SaveChanges();
            return RedirectToAction("AdminProducts");
        }

        // DELETION ----------------------------------------------------

        // Delete Customers

        [HttpGet]
        public IActionResult DeleteCustomer(int id)
        {
            var recordToDelete = _repo.GetCustomerById(id);

            return View(recordToDelete);

        }

        [HttpPost]
        public IActionResult DeleteCustomer(Customer customer)
        {
            _repo.DeleteUser(customer.CustomerId);
            _repo.SaveChanges();

            return RedirectToAction("AdminUsers");
        }

        // Delete Customers

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var recordToDelete = _repo.GetProductById(id);

            return View(recordToDelete);

        }

        [HttpPost]
        public IActionResult DeleteProduct(Product product)
        {
            _repo.DeleteProduct(product.ProductId);
            _repo.SaveChanges();

            return RedirectToAction("AdminProducts");
        }

        // Messing with a User ----------------------------------------

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(IdentityUser user)
        {
            var userToEdit = await _userManager.FindByIdAsync(user.Id);

            if (userToEdit == null)
            {
                return NotFound();
            }

            var result = await _userManager.UpdateAsync(userToEdit);
            if (result.Succeeded)
            {
                return RedirectToAction("AdminUsers", "Home");
            }

            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(IdentityUser user)
        {
            var userToDelete = await _userManager.FindByIdAsync(user.Id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(userToDelete);
            if (result.Succeeded)
            {
                return RedirectToAction("AdminUsers", "Home");
            }

            return RedirectToAction("AdminUsers", "Home");
        }
    }
}
