namespace BrickwellStore.Data.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public List<Product> Recommendations { get; set; }
    }
}