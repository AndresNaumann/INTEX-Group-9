namespace BrickwellStore.Data
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }
        public IQueryable<Customer> Customers { get; }
        public IQueryable<Order> Orders { get; }    
        Customer? GetCustomerById(int id);
        void DeleteCustomer(int id);
        void SaveChanges();


    }
}