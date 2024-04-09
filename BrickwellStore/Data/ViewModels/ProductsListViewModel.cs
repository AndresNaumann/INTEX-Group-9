using BrickwellStore.Data.ViewModels;

namespace BrickwellStore.Data.ViewModels
{
    public class ProductsListViewModel
    {
        public IQueryable<Product> Products { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();

        public string? CurrentProductColor { get; set; }
    }
}
