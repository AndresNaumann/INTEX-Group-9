namespace BrickwellStore.Data
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }
        public IQueryable<Customer> Customers { get; }
        public IQueryable<Order> Orders { get; }    
        Customer? GetCustomerById(int id);
        Customer? GetCustomerByUserId(string userId);
        Product? GetProductById(int id);
        IQueryable<Product> GetRecommendations(int productId);
        IEnumerable<Product> GetCustomerRecommendations(int customerId);
        void AddProduct(Product product);
        void AddUser(Customer customer);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        void UpdateUser(int id);
        void DeleteUser(int id);
        void SaveChanges();
        //Cart? GetCartLine(int cartLineId);
        
    }
}