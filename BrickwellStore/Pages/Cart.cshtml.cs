using BrickwellStore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BrickwellStore.Data;
using System.Diagnostics;

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

        //public void OnGet(string returnUrl)
        //{
        //    ReturnUrl = returnUrl ?? "/";
        //    Cart = HttpContext.Session.GetJson<Cart>("cart");

        //    if (Cart == null || Cart.Lines == null)
        //    {
        //        If Cart or its Lines collection is null, initialize a new Cart object
        //       Cart = new Cart();
        //    }
        //}

        public IActionResult OnPost(int productId, string returnUrl)
        {
            Product p = _repo.Products
             .FirstOrDefault(x => x.ProductId == productId);

            if (p != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(p, 1, p.Price);
                HttpContext.Session.SetJson("cart", Cart);
            }

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        //public IActionResult OnPostRemoveItem(int productId)
        //{
        //    Product productToRemove = _repo.Products.FirstOrDefault(p => p.ProductId == productId);

        //    if (productToRemove != null)
        //    {
        //        Cart.RemoveLine(productToRemove);
        //        HttpContext.Session.SetJson("cart", Cart);
        //    }

        //    return RedirectToPage("/Cart");
        //}
        //    public IActionResult OnPost(int productId, string returnUrl)
        //    {
        //        Product p = _repo.Products.FirstOrDefault(x => x.ProductId == productId);

        //        if (p != null)
        //        {
        //            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        //            Cart.AddItem(new Cart.CartLine { Product = p, Quantity = 1 });
        //            HttpContext.Session.SetJson("cart", Cart);
        //        }

        //        return RedirectToPage(new { returnUrl = returnUrl });
        //    }

        //}
    }
}
