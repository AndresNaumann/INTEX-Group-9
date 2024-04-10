using BrickwellStore.Data;
using BrickwellStore.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BrickwellStore.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace BrickwellStore.Controllers
{
    public class HomeController : Controller
    {
        private ILegoRepository _repo;
        private UserManager<IdentityUser> _userManager;

        public HomeController(ILegoRepository temp, UserManager<IdentityUser> userManager)
        {
            _repo = temp;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            int defaultCustomerId = 10;
            int customerId = defaultCustomerId;

            // Assuming that if the user is logged in, User.Identity will have the details.
            // Logging for debugging purposes.
            if (User.Identity.IsAuthenticated)
            {
                // Log the identity name for debugging
                Debug.WriteLine($"User.Identity.Name: {User.Identity.Name}");

                if (User.Identity.Name == "naumannadn@gmail.com") // Ensure this matches exactly
                {
                    customerId = 1;
                }
            }
            else
            {
                // If we're here, the user is not authenticated. Log this information.
                Debug.WriteLine("User is not authenticated.");
            }

            var recommendations = _repo.GetCustomerRecommendations(customerId);

            // Log the number of recommendations for debugging
            Debug.WriteLine($"Number of recommendations: {recommendations.Count()}");

            return View(recommendations);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View(); 
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }


        public IActionResult ThankYou()
        {
            return View();
        }


        [Authorize]
        public IActionResult Secrets()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FinishCheckout()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            string userId = currentUser?.Id;

            var curCustomer = _repo.GetCustomerByUserId(userId);

            if (curCustomer != null)
            {
                return View(curCustomer);
            } 
            else
            {
                var model = new Customer
                {
                    UserId = userId
                };

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult FinishCheckout(Customer customer)
        {
            var curCustomer = _repo.GetCustomerByUserId(customer.UserId);

            if (curCustomer != null)
            {
                _repo.UpdateUser(curCustomer.CustomerId);
                _repo.SaveChanges();

            } else
            {
                _repo.AddUser(customer);
                _repo.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Product

        public IActionResult Product(int pageNum, string? productColor, string? productCategory, int? itemsPerPage)
        {
            int defaultPageSize = 5;

            int pageSize = itemsPerPage ?? defaultPageSize;

            var productsQuery = _repo.Products
                .Where(x => (x.PrimaryColor == productColor || x.SecondaryColor == productColor) || productColor == null);

            if (!string.IsNullOrEmpty(productCategory))
            {
                productsQuery = productsQuery.Where(x => x.Category == productCategory);
            }

            var Blah = new ProductsListViewModel
            {
                Products = productsQuery
                    .OrderBy(x => x.PrimaryColor == productColor ? 0 : 1) // Order by primary color first
                    .ThenBy(x => x.Name) // Then order by name
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = productColor == null
                        ? _repo.Products.Count()
                        : productsQuery.Count()
                },

                CurrentProductColor = productColor,
                CurrentProductCategory = productCategory
            };

            return View(Blah);
        }


        // ADDING A PRODUCT -------------------------------------------



        // EDITING ----------------------------------------------------

        // Edit a Customer/User

        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            var custToEdit = _repo.GetCustomerById(id);
            return View(custToEdit);

            //var recordToEdit = _repo.GetCustomerById(id);
            //return View(recordToEdit);
        }

        [HttpPost]
        public IActionResult EditUser(IdentityUser updatedInfo)
        {

            _userManager.UpdateAsync(updatedInfo);

            return RedirectToAction("AdminUsers");
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




    }
}
