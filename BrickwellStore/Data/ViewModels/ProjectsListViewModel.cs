using BrickwellStore.Data.ViewModels;

namespace BrickwellStore.Data.ViewModels
{
    public class ProjectsListViewModel
    {
        public IQueryable<Product> Products { get; set; }

        public IQueryable<Customer> Customers { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();

        public string? CurrentProductColor { get; set; }
    }
}
