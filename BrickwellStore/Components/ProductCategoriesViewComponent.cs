using BrickwellStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace BrickwellStore.Components
{
    public class ProductCategoriesViewComponent :ViewComponent
    {
        private ILegoRepository _legorepo;
        public ProductCategoriesViewComponent(ILegoRepository temp)
        {
            _legorepo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProductCategory = RouteData?.Values["productCategory"];
            var productCategories = _legorepo.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return View(productCategories);

        }


    }
}












