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

        public IQueryable<LineItem> LineItems => _context.LineItems;

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

        public async Task<Customer> GetCustomerByIdAsync(int id)
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

        public void UpdateUser(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
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

        public Product? GetProductById(int id)
        {
            try
            {
                var product = _context.Products.Single(c => c.ProductId == id);
                return product;
            }
            catch (InvalidOperationException)
            {
                // Handle the case where the customer is not found
                return null;
            }
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

        // ORDER METHODS

        public Order? GetOrderById(int id)
        {
            try
            {
                var order = _context.Orders.Single(c => c.TransactionId == id);
                return order;
            }
            catch (InvalidOperationException)
            {
                // Handle the case where the customer is not found
                return null;
            }
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
        }
           
        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var order = GetOrderById(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
        }

        public void ApproveOrder(int id)
        {
            var order = GetOrderById(id);
            if (order != null)
            {
                order.Fraud = false;
            }

        }
        public void CompleteOrder(int id)
        {
            var order = GetOrderById(id);
            if (order != null)
            {
                order.IsCompleted = true;
            }
        }

        // LINEITEM METHODS

        public LineItem? GetLineItemById(int id)
        {
            try
            {
                var lineItem = _context.LineItems.Single(c => c.LineId == id);
                return lineItem;
            }
            catch (InvalidOperationException)
            {
                // Handle the case where the customer is not found
                return null;
            }
        }

        public void AddLineItem(LineItem lineItem)
        {
            _context.LineItems.Add(lineItem);
            _context.SaveChanges();
        }
        public void UpdateLineItem(LineItem lineItem)
        {
            _context.LineItems.Update(lineItem);
            _context.SaveChanges();
        }

        public void DeleteLineItem(int id)
        {
            var item = GetLineItemById(id);

            if (item != null)
            {
                _context.LineItems.Remove(item);
            }
        }

        public List<LineItem> GetLineItems(int id)
        {
            return _context.LineItems.Where(i => i.TransactionId == id).ToList();
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


