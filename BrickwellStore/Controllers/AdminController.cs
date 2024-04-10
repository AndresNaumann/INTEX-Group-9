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
        // Bring in these tables and configure viewmodels later to combine the data into a presentable format

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

        // Allows the Admin to see all users and drill into each one

        [Authorize(Roles = "Admin")]
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

        // See all the products

        [Authorize(Roles = "Admin")]
        public IActionResult AdminProducts(int pageNum)
        {
            int pageSize = 10;
            var AdminBlah = new ProductsListViewModel
            {
                Products = _repo.Products
                .OrderBy(x => x.Name)
               .Skip((pageNum - 1) * pageSize)
               .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Products.Count()
                },
            };

            return View(AdminBlah);
        }

        // See all the orders as an admin

        [Authorize(Roles = "Admin")]
        public IActionResult AdminOrders(int pageNum)
        {
            int pageSize = 10;
            var AdminBlah = new ProductsListViewModel
            {
                Orders = _repo.Orders
                .OrderBy(x => x.Date)
               .Skip((pageNum - 1) * pageSize)
               .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Orders.Count()
                },
            };

            return View(AdminBlah);
        }

        // Returns roles of all the users

        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
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

        //[HttpGet]
        //public IActionResult EditProduct(int id)
        //{
        //    var recordToEdit = _repo.GetProductById(id);

        //    return View(recordToEdit);
        //}

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


        // Editing or Deleting a User ----------------------------------------

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
