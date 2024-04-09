
using Microsoft.AspNetCore.Mvc;
using BrickwellStore.Data;

namespace BrickwellStore.Components
{
    public class ProjectTypesViewComponent : ViewComponent
    {
        private ILegoRepository _legorepo;
        public ProjectTypesViewComponent(ILegoRepository temp)
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
            return View(productColors);
        }
    }
}





