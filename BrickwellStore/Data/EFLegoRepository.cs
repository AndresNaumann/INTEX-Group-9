using static BrickwellStore.Data.Cart;

namespace BrickwellStore.Data
{
    public class EFLegoRepository : ILegoRepository
    {
        private BrickwellContext _context;
        public EFLegoRepository(BrickwellContext temp)
        {
            _context = temp;
        }

        // BRING OUT ALL THE TABLES

        public IQueryable<Product> Products => _context.Products;
        public IQueryable<Customer> Customers => _context.Customers;
        public IQueryable<Order> Orders => _context.Orders;

        // USER METHODS

        public void AddUser(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.Single(c => c.CustomerId == id);
        }

        public Customer GetCustomerByUserId(string  userId)
        {
            return _context.Customers.Single(c => c.UserId == userId);
        }

        public void UpdateUser(int id)
        {
            var customer = GetCustomerById(id);
            _context.Customers.Update(customer);
        }

        public void DeleteUser(int id)
        {
            var customer = GetCustomerById(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        // PRODUCT METHODS

        public Product GetProductById(int id)
        {
            return _context.Products.Single(c => c.ProductId == id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }

        // METHOD TO SAVE CHANGES

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        //public IQueryable<CartLine> GetCartLines(string userId, string color = null, int? categoryId = null)
        //{
        //    var query = _context.CartLines
        //        .Where(cl => cl.UserId == userId); // Filter by user ID

        //    if (!string.IsNullOrEmpty(color))
        //    {
        //        // Filter by color (assuming you have a Color property in CartLine)
        //        query = query.Where(cl => cl.Color == color);
        //    }

        //    if (categoryId.HasValue)
        //    {
        //        // Filter by category (assuming you have a CategoryId property in CartLine)
        //        query = query.Where(cl => cl.CategoryId == categoryId.Value);
        //    }

        //    return query;
        //}

        //public void UpdateCartLineQuantity(int cartLineId, int quantity)
        //{
        //    var cartLine = GetCartLine(cartLineId);

        //    if (cartLine != null)
        //    {
        //        // Validate the quantity (ensure it's greater than zero)
        //        if (quantity > 0)
        //        {
        //            //cartLine.Quantity = quantity;
        //            // Save changes to the database (if needed)
        //            _context.SaveChanges();
        //        }
        //        else
        //        {
        //            // Handle invalid quantity (e.g., log an error or show a message)
        //            // You can customize this part based on your application's requirements
        //        }
        //    }
        //}
    }
}


