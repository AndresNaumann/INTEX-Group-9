using BrickwellStore.Data;
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

            return View(product);
        }

        public IActionResult AdminProductDetails(int id)
        {
            var product = _repo.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound(); // Or handle accordingly
            }

            return View(product);
        }
    }
}
