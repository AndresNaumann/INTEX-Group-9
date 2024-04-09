using BrickwellStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrickwellStore.Controllers
{
    public class AdminController : Controller
    {
        private ILegoRepository _repo;
        private UserManager<IdentityUser> _userManager;

        public AdminController(ILegoRepository temp, UserManager<IdentityUser> userManager)
        {
            _repo = temp;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
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
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            //var result = await _userManager.DeleteAsync(user);

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
