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
using Microsoft.AspNetCore.Hosting;

namespace BrickwellStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly InferenceSession _session;
        private readonly ILogger<HomeController> _logger;
        private ILegoRepository _repo;
        private UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly Cart _cart;
        


        public HomeController(ILegoRepository temp, UserManager<IdentityUser> userManager, ILogger<HomeController> logger, IWebHostEnvironment environment, Cart cart)
        {
            _repo = temp;
            _userManager = userManager;
            _logger = logger;
            _environment = environment;
            _cart = cart;


            string modelPath = Path.Combine(_environment.ContentRootPath, "fraud_model.onnx");

            try
            {
                _session = new InferenceSession(modelPath);
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

        public IActionResult AboutUs()
        {
            return View();
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }


        public IActionResult ThankYou()
        {
            return View();
        }

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

                _repo.AddUser(model);
                _repo.SaveChanges();

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> FinishCheckout(Customer customer, Order order)
        {
            var curCustomer = _repo.GetCustomerByUserId(customer.UserId);

            _repo.UpdateUser(customer.CustomerId);
            _repo.SaveChanges();

            if (curCustomer == null)
            {
                _repo.AddUser(customer);
                _repo.SaveChanges();
            }

            int time = DateTime.Now.Hour;
            // put cart total amount right here
            float amount = (float)_cart.CalculateTotal();
            int country_of_transaction_United_Kingdom = customer.Country == "England" ? 1 : 0;
            int shipping_address_United_Kingdom = country_of_transaction_United_Kingdom;

            string fraudPrediction = PredictFraud(time, amount, country_of_transaction_United_Kingdom, shipping_address_United_Kingdom);
            TempData["Prediction"] = fraudPrediction;

            DateTime date = DateTime.Now;
            string formattedDate = date.ToString("MM/dd/yyyy");

            bool isFraud = false;

            if (fraudPrediction == "Fraud")
            {
                isFraud = true;
            }

            var updatedCustomer = await _repo.GetCustomerByIdAsync(customer.CustomerId);

            var newOrder = new Order
            {
                CustomerId = curCustomer.CustomerId,
                Amount = (float)amount,
                Date = formattedDate,
                TransactionType = "Credit Card",
                TransactionCountry = updatedCustomer.Country,
                ShippingAddress = updatedCustomer.Address1 + " " + updatedCustomer.Address2 + ", " + updatedCustomer.City + ", " + updatedCustomer.State + " " + updatedCustomer.Zip,
                Fraud = isFraud,
                IsCompleted = false,
            };

            _cart.Clear();

            if (fraudPrediction == "Fraud")
            {
                _repo.AddOrder(newOrder);
                _repo.SaveChanges();
                return View("PendingTransaction", newOrder);
            }
            else if (fraudPrediction == "Not Fraud")
            {
                _repo.AddOrder(newOrder);
                _repo.SaveChanges();
                return View("ThankYou", newOrder);
            }
            else
            { return View("Index"); }

        }

        private string PredictFraud(int time, float amount, int countryOfTransactionUK, int shippingAddressUK)
        {
            var class_type_dict = new Dictionary<int, string>
            {
                {0, "Not Fraud" },
                {1, "Fraud" }
            };

            // Prepare input data for the ONNX model
            var input = new List<float> { time, amount, countryOfTransactionUK, shippingAddressUK };
            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });
            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
            };

            // Run the model with the input data
            using (var results = _session.Run(inputs))
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                if (prediction != null && prediction.Length > 0)
                {
                    return class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown");
                }
                else
                {
                    return "Error: Unable to make a prediction";
                }
            }
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

        public async Task<IActionResult> OrderHistory(int pageNum)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            string userId = currentUser?.Id;
            var currentCustomer = _repo.GetCustomerByUserId(userId);


            int pageSize = 10;
            var MyOrders = new ProductsListViewModel
            {
                Orders = _repo.Orders
                .Where(u => u.CustomerId == currentCustomer.CustomerId)
                .OrderBy(x => x.Date)
               .Skip((pageNum - 1) * pageSize)
               .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Orders.Count(u => u.CustomerId == currentCustomer.CustomerId)
                },
            };

            return View(MyOrders);
        }


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
