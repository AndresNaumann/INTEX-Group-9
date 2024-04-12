using BrickwellStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrickwellStore.Controllers
{
    public class ProductController : Controller
    {
        private ILegoRepository _repo;

        public ProductController(ILegoRepository temp)
        {
            _repo = temp;
        }
        public IActionResult ProductDetails(int id)
        {
            var product = _repo.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound(); // Or handle accordingly
            }

            var recommendations = _repo.GetRecommendations(id).ToList();

            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                Recommendations = recommendations
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminProductDetails(int id)
        {
            var product = _repo.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound(); // Or handle accordingly
            }

            return View(product);
        }

        // ADDING A PRODUCT TO THE DATABASE

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _repo.AddProduct(product);
            _repo.SaveChanges();

            return RedirectToAction("AdminProducts", "Admin");
        }

        // EDITING A PRODUCT

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _repo.GetProductById(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            _repo.UpdateProduct(product);
            _repo.SaveChanges();

            return RedirectToAction("AdminProducts", "Admin");
        }

        // DELETING A PRODUCT

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

            return RedirectToAction("AdminProducts", "Admin");
        }

        [HttpGet]
        public IActionResult ViewLineItems(int id)
        {
            var items = _repo.LineItems.Where(i => i.TransactionId == id).ToList();
            return View(items);
        }

    }
}
