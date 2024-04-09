//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using BrickwellStore.Data; // Assuming you have a Cart and Product model

//namespace BrickwellStore.Pages
//{
//    public class EditCartModel : PageModel
//    {
//        private readonly ILegoRepository _repo;

//        public EditCartModel(ILegoRepository repo)
//        {
//            _repo = repo;
//        }

//        [BindProperty]
//        public Cart? Cart { get; set; } // Assuming you have a CartLine model

//        public IActionResult OnGet(int cartLineId, string returnUrl)
//        {
//            // Fetch the cart line based on cartLineId (similar to your existing logic)
//            Cart = _repo.GetCartLine(cartLineId);

//            if (Cart == null)
//            {
//                // Handle case when cart line is not found (e.g., show an error message)
//                return NotFound();
//            }

//            ViewData["ReturnUrl"] = returnUrl ?? "/";
//            return Page();
//        }

//        public IActionResult OnPost(string returnUrl)
//        {
//            if (ModelState.IsValid)
//            {
//                // Update the cart line quantity
//                _repo.UpdateCartLineQuantity(Cart.CartLineId, Cart.Quantity);
//                // Redirect back to the cart page
//                return RedirectToPage("/Cart", new { returnUrl });
//            }

//            // If validation fails, stay on the edit page
//            ViewData["ReturnUrl"] = returnUrl ?? "/";
//            return Page();
//        }
//    }
//}
