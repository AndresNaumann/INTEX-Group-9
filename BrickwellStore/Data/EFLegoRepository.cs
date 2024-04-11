using static BrickwellStore.Data.Cart;
using System.Diagnostics;

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

        public Customer? GetCustomerById(int id)
        {
            try
            {
                var customer = _context.Customers.Single(c => c.CustomerId == id);
                return customer;
            }
            catch (InvalidOperationException)
            {
                // Handle the case where the customer is not found
                return null;
            }
        }

        public Customer? GetCustomerByUserId(string userId)
        {
            try
            {
                var customer = _context.Customers.Single(c => c.UserId == userId);
                return customer;
            }
            catch (InvalidOperationException)
            {
                // Handle the case where the customer is not found
                return null;
            }
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

        public IQueryable<Product> GetRecommendations(int productId)
        {
            var recommendedProductIds = _context.ProductRecommendations.Where(pr => pr.ProductID == productId).Select(pr => pr.RecommendedProductID);

            var recommendedProducts = _context.Products.Where(p => recommendedProductIds.Contains(p.ProductId)).AsQueryable();

            return recommendedProducts;
        }

        public IEnumerable<Product> GetCustomerRecommendations(int customerId)
        {
            var recommendedProductIds = _context.CustomerRecommendations
                .Where(cr => cr.CustomerId == customerId)
                .Select(cr => cr.RecommendedProductId)
                .ToList();

            // Debugging: check the count and the actual IDs being retrieved
            Debug.WriteLine($"Total recommended product IDs: {recommendedProductIds.Count}");
            foreach (var id in recommendedProductIds)
            {
                Debug.WriteLine($"Recommended Product ID: {id}");
            }

            var recommendedProducts = _context.Products
                .Where(p => recommendedProductIds.Contains(p.ProductId))
                .ToList();

            // Debugging: check the count of the products returned
            Debug.WriteLine($"Total recommended products found: {recommendedProducts.Count}");

            return recommendedProducts;
        }

    }
}


