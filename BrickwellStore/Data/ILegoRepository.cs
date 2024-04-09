namespace BrickwellStore.Data
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }
        public IQueryable<Customer> Customers { get; }
        public IQueryable<Order> Orders { get; }   
        
        Customer? GetCustomerById(int id);
        Product? GetProductById(int id);
        void AddProduct(Product product);
        void DeleteCustomer(int id);
        void DeleteProduct(int id);
        void SaveChanges();


    }
}