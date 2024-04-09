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

        [Authorize(Roles = "Admin")]
        public IActionResult Secrets()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminProducts(int pageNum)
        {
            int pageSize = 10;
            var AdminBlah = new ProjectsListViewModel
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
        public IActionResult AdminUsers(int pageNum)
        {
            int pageSize = 5;
            var AdminUsers = new ProjectsListViewModel
            {
                Customers = _repo.Customers
                 .OrderBy(x => x.CustomerFirstName)
               .Skip((pageNum - 1) * pageSize)
               .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Customers.Count()
                },
            };

            return View(AdminUsers);
        }

        public IActionResult Product(int pageNum, string? productColor)
        {

            int pageSize = 5;
            var Blah = new ProjectsListViewModel
            {
                Products = _repo.Products
                .Where(x => x.PrimaryColor == productColor || productColor == null)
               .OrderBy(x => x.Name)
               .Skip((pageNum - 1) * pageSize)
               .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = productColor == null ? _repo.Products.Count() : _repo.Products.Where(x => x.PrimaryColor == productColor).Count()


                },

                CurrentProductColor = productColor
            };

            return View(Blah);

        }
    }
}
