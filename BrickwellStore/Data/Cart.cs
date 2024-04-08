namespace BrickwellStore.Data
{
    //public class Cart
    //{
    //    public List<CartLine> Lines { get; set; } = new List<CartLine>();

    //    public void AddItem(Project p, int quantity)
    //    {
    //        CartLine? line = Lines
    //            .Where(x => x.Project.ProjectId == p.ProjectId)
    //            .FirstOrDefault();

    //        //has this item already been added to our cart
    //        if (line == null)
    //        {
    //            Lines.Add(new CartLine
    //            {
    //                Project = p,
    //                Quantity = quantity
    //            });
    //        }
    //        else
    //        {
    //            line.Quantity += quantity;
    //        }
    //    }

    //    public void RemoveLine(Project p) => Lines.RemoveAll(x => x.Project.ProjectId == p.ProjectId);

    //    public void Clear() => Lines.Clear();

    //    public decimal CalculateTotal() => Lines.Sum(x => 25 * x.Quantity);

    //    public class CartLine
    //    {
    //        public int CartLineId { get; set; }
    //        public Project Project { get; set; } = new();
    //        public int Quantity { get; set; }

    //    }
    }
}