namespace BrickwellStore.Data
{
    public class ILegoRepository
    {
        public IQueryable<Product> Products { get; }
    }
}