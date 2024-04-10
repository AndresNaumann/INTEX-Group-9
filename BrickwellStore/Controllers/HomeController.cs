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
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SQLitePCL;

namespace BrickwellStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly InferenceSession _session;
        private readonly ILogger<HomeController> _logger;
        private ILegoRepository _repo;
        private UserManager<IdentityUser> _userManager;

        public HomeController(ILegoRepository temp, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _repo = temp;
            _userManager = userManager;
            _logger = logger;

            try
            {
                _session = new InferenceSession("\\\\Mac\\Home\\Documents\\GitHub\\INTEX-Group-9\\BrickwellStore\\fraud_model.onnx");
                _logger.LogInformation("NNX model loaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading the ONNX model: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Predict(int time, float amount, int country_of_transaction_United_Kingdom, int shipping_address_United_Kingdom)
        {
            // Mapping of class type to a readable format
            var class_type_dict = new Dictionary<int, string>
            {
                {0, "Not Fraud" },
                {1, "Fraud" }
            };

            try
            {
                // Prepare input data for the ONNX model
                var input = new List<float> { time, amount, country_of_transaction_United_Kingdom, shipping_address_United_Kingdom };
                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });
                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };

                // Run the model with the input data
                using (var results = _session.Run(inputs))
                {
                    // Retrieve the prediction result
                    var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                    if (prediction != null && prediction.Length > 0)
                    {
                        // Map the numerical result to a meaningful string
                        var fraudType = class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown");
                        TempData["Prediction"] = fraudType;
                    }
                    else
                    {
                        TempData["Prediction"] = "Error: Unable to make a prediction";
                    }
                }

                // Return the view with the prediction result
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle exceptions and return error message
                return BadRequest($"Error: {ex.Message}");
            }
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

        public IActionResult ProductDetail(int productId)
        {
            var product = _repo.GetProductById(productId);
            var recommendations = _repo.GetRecommendations(productId);

            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                Recommendations = recommendations.ToList()
            };

            return View(viewModel);
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

        // EDIT CART ITEMS 

        public IActionResult EditCartItem(int cartLineId, int quantity)
        {
            var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            var cartLine = cart.Lines.FirstOrDefault(x => x.CartLineId == cartLineId);

            if (cartLine != null && quantity > 0)
            {
                cartLine.Quantity = quantity;
                HttpContext.Session.SetJson("cart", cart);
            }

            return RedirectToPage("/Cart");
        }


        // DELETE CART ITEM
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
