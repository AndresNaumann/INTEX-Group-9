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

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.Single(c => c.CustomerId == id);
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

        public void UpdateProduct(int id)
        {
            var product = GetProductById(id);
            _context.Products.Update(product);
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
    }
}


