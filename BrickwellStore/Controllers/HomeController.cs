using BrickwellStore.Data;
using BrickwellStore.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace BrickwellStore.Controllers
{
    public class HomeController : Controller
    {
        private ILegoRepository _repo;

        public HomeController(ILegoRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

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

        //public IActionResult Product(int pageNum, string? productColor)
        //{

        //    int pageSize = 5;
        //    var Blah = new ProductsListViewModel
        //    {
        //        Products = _repo.Products
        //        .Where(x => x.PrimaryColor == productColor || productColor == null)
        //       .OrderBy(x => x.Name)
        //       .Skip((pageNum - 1) * pageSize)
        //       .Take(pageSize),

        //        PaginationInfo = new PaginationInfo
        //        {
        //            CurrentPage = pageNum,
        //            ItemsPerPage = pageSize,
        //            TotalItems = productColor == null ? _repo.Products.Count() : _repo.Products.Where(x => x.PrimaryColor == productColor).Count()


        //        },

        //        CurrentProductColor = productColor
        //    };

        //    return View(Blah);

        //}

        public IActionResult Product(int pageNum, string? productColor, int pageSize = 5)
        {
            var filteredProducts = _repo.Products
                .Where(x => x.PrimaryColor == productColor || productColor == null)
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);

            var viewModel = new ProductsListViewModel
            {
                Products = filteredProducts,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = productColor == null ? _repo.Products.Count() : _repo.Products.Count(x => x.PrimaryColor == productColor)
                },
                CurrentProductColor = productColor
            };

            return View(viewModel);
        }

    }
}
