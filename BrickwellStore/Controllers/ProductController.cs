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

            var recommendations = _repo.GetRecommendations(id).ToList();

            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                Recommendations = recommendations
            };

            return View(viewModel);
        }
    }
}
