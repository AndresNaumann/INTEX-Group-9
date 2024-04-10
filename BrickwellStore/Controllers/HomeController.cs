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
            return View();
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FinishCheckout()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            string userId = currentUser?.Id;

            var model = new Customer
            {
                UserId = userId
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult FinishCheckout(Customer customer)
        {
            _repo.AddUser(customer);
            _repo.SaveChanges();

            return RedirectToAction("Index");
        }


        //public IActionResult AdminUsers(int pageNum)
        //{
        //    int pageSize = 5;
        //    var AdminUsers = new ProductsListViewModel
        //    {
        //        Customers = _repo.Customers
        //         .OrderBy(x => x.CustomerFirstName)
        //       .Skip((pageNum - 1) * pageSize)
        //       .Take(pageSize),

        //        PaginationInfo = new PaginationInfo
        //        {
        //            CurrentPage = pageNum,
        //            ItemsPerPage = pageSize,
        //            TotalItems = _repo.Customers.Count()
        //        },
        //    };

        //    return View(AdminUsers);
        //}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

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

        //[HttpGet]
        //public IActionResult EditUser(string id)
        //{
        //    var userToEdit = _userManager.Users.Single(x => x.Id == id);
        //    return View(userToEdit);

        //    //var recordToEdit = _repo.GetCustomerById(id);
        //    //return View(recordToEdit);
        //}

        //[HttpPost]
        //public IActionResult EditUser(IdentityUser updatedInfo)
        //{

        //    _userManager.UpdateAsync(updatedInfo);

        //    //_repo.UpdateUser(updatedInfo.CustomerId);
        //    //_repo.SaveChanges();

        //    return RedirectToAction("AdminUsers");
        //}

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

            //_repo.UpdateUser(updatedInfo.CustomerId);
            //_repo.SaveChanges();

            return RedirectToAction("AdminUsers");
        }

        // Edit a Product

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var recordToEdit = _repo.GetProductById(id);
            return View("AddProduct", recordToEdit);
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

        // EDIT CART ITEMS

        public IActionResult EditCartItem(int cartLineId, int quantity)
        {
            // Retrieve the cart from the session or create a new one
            var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

            // Find the cart line with the specified CartLineId
            var cartLine = cart.Lines.FirstOrDefault(x => x.CartLineId == cartLineId);

            if (cartLine != null)
            {
                // Validate the quantity (ensure it's greater than zero)
                if (quantity > 0)
                {
                    cartLine.Quantity = quantity;
                    HttpContext.Session.SetJson("cart", cart);
                }
                else
                {
                    // Handle invalid quantity (e.g., display an error message)
                    // You can customize this part based on your application's requirements
                    // For now, let's assume you log an error or show a user-friendly message
                    // indicating that the quantity must be a positive value.
                }
            }
            else
            {
                // Handle case when cart line is not found (e.g., log an error)
                // You can customize this part based on your application's requirements
                // For example, you might want to redirect the user to an error page.
            }

            // Redirect to the "/Cart" page
            return RedirectToPage("/Cart");
        }



        //DELETE CART ITEM
        public IActionResult DeleteCartItem(int cartLineId)
        {
            var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            var cartLine = cart.Lines.FirstOrDefault(x => x.CartLineId == cartLineId);

            if (cartLine != null)
            {
                cart.Lines.Remove(cartLine);
                HttpContext.Session.SetJson("cart", cart);
            }
            else
            {
                // Handle the case where the specified cartLineId is not found (optional).
                // You can add logging or display an error message.
            }

            return RedirectToPage("/Cart");
        }



    }
}
