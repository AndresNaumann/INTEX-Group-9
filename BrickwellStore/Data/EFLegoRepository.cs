namespace BrickwellStore.Data
{
    public class EFLegoRepository : ILegoRepository
    {
        private BrickwellContext _context;
        public EFLegoRepository(BrickwellContext temp)
        {
            _context = temp;
        }
        public IQueryable<Product> Products => _context.Products;
        public IQueryable<Customer> Customers => _context.Customers;
        public IQueryable<Order> Orders => _context.Orders;

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.Single(c => c.CustomerId == id);
        }

        public Product GetProductById(int id)
        {
            return _context.Products.Single(c => c.ProductId == id);
        }

        public void DeleteCustomer(int id)
        {
            var customer = GetCustomerById(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}


