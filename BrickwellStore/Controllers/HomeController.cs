using BrickwellStore.Data;
using BrickwellStore.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public IActionResult Product(int pageNum, string? projectType)
        {

            //int pageSize = 5;
            //var Blah = new ProjectsListViewModel
            //{
            //    Projects = _repo.Projects
            //    .Where(x => x.ProjectType == projectType || projectType == null)
            //   .OrderBy(x => x.ProjectName)
            //   .Skip((pageNum - 1) * pageSize)
            //   .Take(pageSize),

            //    PaginationInfo = new PaginationInfo
            //    {
            //        CurrentPage = pageNum,
            //        ItemsPerPage = pageSize,
            //        TotalItems = projectType == null ? _repo.Projects.Count() : _repo.Projects.Where(x => x.ProjectType == projectType).Count()


            //    },

            //    CurrentProjectType = projectType
            };

            return View(Blah);

        }
    }
}
