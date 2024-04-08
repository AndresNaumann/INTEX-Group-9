namespace BrickwellStore.Data
{
    public class EFLegoRepository : ILegoRepository
    {
        private WaterProjectContext _context;
        public EFLegoRepository(WaterProjectContext temp)
        {
            _context = temp;
        }
        public IQueryable<Project> Projects => _context.Projects;
    }
}
}

