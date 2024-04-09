using BrickwellStore.Data;
using BrickwellStore.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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

        [Authorize(Roles = "Admin")]
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult ProductDetail()
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

        //public IActionResult Product(int pageNum, string? productColor, string? productCategory)
        //{

        //    int pageSize = 5;
        //    var Blah = new ProductsListViewModel
        //    {
        //        Products = _repo.Products
        //        .Where(x => x.PrimaryColor == productColor || x.SecondaryColor == productColor || productColor == null)
        //       .OrderBy(x => x.Name)
        //       .Skip((pageNum - 1) * pageSize)
        //       .Take(pageSize),

        //        PaginationInfo = new PaginationInfo
        //        {
        //            CurrentPage = pageNum,
        //            ItemsPerPage = pageSize,
        //            TotalItems = productColor == null ? _repo.Products.Count() : _repo.Products.Where(x => x.PrimaryColor == productColor).Count()


        //        },

        //        CurrentProductColor = productColor,
        //        CurrentProductCategory = productCategory
        //    };

        //    return View(Blah);

        //}
        public IActionResult Product(int pageNum, string? productColor, string? productCategory)
        {
            int pageSize = 5;
            var query = _repo.Products.AsQueryable();

            // Apply color filter if provided
            if (!string.IsNullOrEmpty(productColor))
            {
                query = query.Where(x => x.PrimaryColor == productColor || x.SecondaryColor == productColor);
            }

            // Apply category filter if provided
            if (!string.IsNullOrEmpty(productCategory))
            {
                query = query.Where(x => x.Category == productCategory);
            }

            var products = query.OrderBy(x => x.Name)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            var viewModel = new ProductsListViewModel
            {
                Products = _repo.Products,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = query.Count() // Count total items from the filtered query
                },
                CurrentProductColor = productColor,
                CurrentProductCategory = productCategory
            };

            return View(viewModel);
        }

        //public IActionResult Product(int pageNum, string? productColor, string? productCateogry, int pageSize = 5)
        //{
        //    var filteredProducts = _repo.Products
        //        .Where(x => x.PrimaryColor == productColor || x.SecondaryColor == productColor || productColor == null)
        //        .OrderBy(x => x.Name)
        //        .Skip((pageNum - 1) * pageSize)
        //        .Take(pageSize);


        //    var viewModel = new ProductsListViewModel
        //    {
        //        Products = filteredProducts,
        //        PaginationInfo = new PaginationInfo
        //        {
        //            CurrentPage = pageNum,
        //            ItemsPerPage = pageSize,
        //            TotalItems = productColor == null ? _repo.Products.Count() : _repo.Products.Count(x => x.PrimaryColor == productColor)
        //        },
        //        CurrentProductColor = productColor
        //    };

        //    return View(viewModel);
        //}

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

            return View("AdminProduct", product);
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
            _repo.DeleteCustomer(updatedInfo.CustomerId);
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
            _repo.DeleteCustomer(updatedInfo.ProductId);
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
            _repo.DeleteCustomer(customer.CustomerId);
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

    }
}
