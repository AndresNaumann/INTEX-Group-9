using BrickwellStore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BrickwellStore.Data;

namespace BrickwellStore.Pages
{
    public class CartModel : PageModel
    {
        private ILegoRepository _repo;
        public CartModel(ILegoRepository temp)
        {
            _repo = temp;
        }
        public Cart? Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(int projectId, string returnUrl)
        {
            //Project p = _repo.Projects
            // .FirstOrDefault(x => x.ProjectId == projectId);

            //if (p != null)
            //{
            //    Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            //    Cart.AddItem(p, 1);
            //    HttpContext.Session.SetJson("cart", Cart);
            //}

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
