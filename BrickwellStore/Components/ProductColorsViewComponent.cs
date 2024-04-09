
using Microsoft.AspNetCore.Mvc;
using BrickwellStore.Data;

namespace BrickwellStore.Components
{
    public class ProductColorsViewComponent : ViewComponent
    {
        private ILegoRepository _legorepo;
        public ProductColorsViewComponent(ILegoRepository temp)
        {
            _legorepo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProductColor = RouteData?.Values["productColor"];
            var productColors = _legorepo.Products
                .Select(x => x.PrimaryColor)
                .Distinct()
                .OrderBy(x => x);

            //// Pass page size options
            //var pageSizeOptions = new List<int> { 5, 10, 20 }; // Add more options if needed

            //// Retrieve selected page size (if any)
            //var selectedPageSize = Convert.ToInt32(RouteData?.Values["pageSize"] ?? 10); // Default page size is 10

            //var model = new ProductFiltersViewComponent
            //{
            //    ProductColors = productColors,
            //    PageSizeOptions = pageSizeOptions,
            //    SelectedPageSize = selectedPageSize
            //};

            //return View(model);
            return View(productColors);

        }


    }
}


//using BrickwellStore.Data;
//using Microsoft.AspNetCore.Mvc;

//namespace BrickwellStore.Components
//{
//    public class ProductFiltersViewComponent : ViewComponent
//    {
//        private ILegoRepository _legorepo;
//        public ProductFiltersViewComponent(ILegoRepository temp)
//        {
//            _legorepo = temp;
//        }
//        // Define the model for the view component directly within the namespace
//        public class ProductFiltersModel
//        {
//            public List<string> ProductColors { get; set; }
//            public List<int> PageSizeOptions { get; set; }
//            public int SelectedPageSize { get; set; }
//        }

//        public IViewComponentResult Invoke()
//        {
//            ViewBag.SelectedProductColor = RouteData?.Values["productColor"];

//            var productColors = _legorepo.Products
//                .Select(x => x.PrimaryColor)
//                .Distinct()
//                .OrderBy(x => x)
//                .ToList(); // Ensure to convert to list

//            // Pass page size options
//            var pageSizeOptions = new List<int> { 5, 10, 20 }; // Add more options if needed

//            // Retrieve selected page size (if any)
//            var selectedPageSize = Convert.ToInt32(RouteData?.Values["pageSize"] ?? 10); // Default page size is 10

//            var model = new ProductFiltersModel // Use the model directly within the component
//            {
//                ProductColors = productColors,
//                PageSizeOptions = pageSizeOptions,
//                SelectedPageSize = selectedPageSize
//            };

//            return View(model);
//        }
//    }
//}






