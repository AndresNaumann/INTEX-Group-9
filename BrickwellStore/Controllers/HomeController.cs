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
using System.Threading.Tasks;


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

<<<<<<< HEAD
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
         
=======

>>>>>>> dev
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
