namespace BrickwellStore.Data
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }
        public IQueryable<Customer> Customers { get; }
        public IQueryable<Order> Orders { get; }  

        Customer? GetCustomerById(int id);
        Product? GetProductById(int id);
        IQueryable<Product> GetRecommendations(int productId);
        void AddProduct(Product product);
        void DeleteCustomer(int id);
        void DeleteProduct(int id);
        void SaveChanges();


    }
}