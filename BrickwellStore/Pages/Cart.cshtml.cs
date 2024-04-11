using BrickwellStore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BrickwellStore.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace BrickwellStore.Pages
{
    public class CartModel : PageModel
    {
        private ILegoRepository _repo;
        private UserManager<IdentityUser> _userManager;
        public Cart Cart { get; set; }

        public CartModel(ILegoRepository temp, Cart cartService, UserManager<IdentityUser> user)
        {
            _repo = temp;
            _userManager = user;
            Cart = cartService;
        }

        public string ReturnUrl { get; set; } = "/";

        public async Task OnGetAsync(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            var user = await _userManager.GetUserAsync(User);
            ViewData["UserId"] = user?.Id;
        }
        //public void OnGet(string returnUrl)
        //{
        //    ReturnUrl = returnUrl ?? "/";
        //}


        public IActionResult OnPost(int productId, string returnUrl)
        {
            Product p = _repo.Products
             .FirstOrDefault(x => x.ProductId == productId);

            if (p != null)
            {
                Cart.AddItem(p, 1, p.Price);
            }

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove (int productId, string returnUrl) 
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.ProductId == productId).Product);

            return RedirectToPage(new {returnUrl = returnUrl});
        }
    }
}
